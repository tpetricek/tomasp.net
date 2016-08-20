(* ------------------------------------------------------------------------- *)
(* Joinad type and joinad operations with basic equality axioms              *)
(* ------------------------------------------------------------------------- *)

Require Import Coq.Program.Basics.
Require Import FunctionalExtensionality.
Require Import Setoid.

Variable J : Type -> Type.

Variable unit :
  forall (A : Type),   A -> J A.
Variable bind :
  forall (A B : Type), (A -> J B) -> J A -> J B.

Variable mzip : 
  forall (A B : Type), J A -> J B -> J (A * B).

Variable morelse :
  forall (A : Type),   J A -> J A -> J A.
Variable mzero :
  forall (A : Type),   J A.

Variable mduplicate :
  forall (A : Type),   J A -> J (J A).

(* ------------------------------------------------------------------------- *)
(* Monad & joinad laws                                                       *)
(* ------------------------------------------------------------------------- *)

Notation "a >>= f" := (bind _ _ f a) (at level 60, right associativity).

Definition map {A B : Type} (f : A -> B) a :=
  a >>= (compose (unit _) f).
Definition swap {A B} (arg : A * B) := (snd arg, fst arg).
Definition swap' {A B} (arg : A * B) :=
  let '(a, b) := arg in (b, a).
Definition assoc {A B C} (arg : (A * B) * C) :=
  let '((a, b), c) := arg in (a, (b, c)).
Definition dup {A} (arg : A) := (arg, arg).
Definition flip {A B C} (f : A -> B -> C) := (fun x y => f y x).

(* Monad laws (left/right identity, associativity) *)

Axiom mbind_unit_left :
  forall A B a, forall f : A -> J B,
  (unit _ a) >>= f = f a.
Axiom mbind_unit_right :
  forall A, forall m : J A,
  m >>= (unit _) = m.
Axiom mbind_assoc :
  forall A B C, forall m : J A, forall f : A -> J B, forall g : B -> J C,
  (m >>= f) >>= g = m >>= (fun x => f x >>= g).

(* MonadZero laws (left/right zero) *)

Axiom mbind_zero_left : 
  forall A B f,
  mzero A >>= f = mzero B.
Axiom mbind_zero_right :
  forall A B, forall (m : J A),
  m >>= (fun _ => mzero B) = mzero B.


(* MonadZip laws (symmetry, naturality, product, duplication, zero) *)
Axiom mzip_assoc : 
  forall A B C a b c, 
  mzip A _ a (mzip B C b c) = (mzip _ _ (mzip _ _ a b) c) >>= (compose (unit _) assoc).
Axiom mzip_sym : 
  forall A B a b, 
  mzip A B a b = (mzip _ _ b a) >>= (compose (unit _) swap).
Axiom mzip_naturality :
  forall A B C D a b, forall (f : A -> B), forall (g : C -> D),
  mzip _ _ (map f a) (map g b) = map (fun x => (f (fst x), g (snd x))) (mzip _ _ a b).
Axiom mzip_product :
  forall A B a b,
  mzip A B (unit _ a) (unit _ b) = unit _ (a, b).
Axiom mzip_duplication :
  forall A a,
  mzip A A a a = map dup a.
Axiom mzip_zero :
  forall A B a,
  mzip A B a (mzero _) = mzero _.


(* MonadOr laws (associativity, left/right unit, left bias) *)

Axiom morelse_assoc : (* associativity is not used - only to hide internals of the translation *)
  forall A a b c,
  morelse A (morelse _ a b) c = morelse _ a (morelse _ b c).
Axiom morelse_unit_right :
  forall A a,
  morelse A a (mzero _) = a.
Axiom morelse_unit_left :
  forall A a,
  morelse A (mzero _) a = a.
Axiom morelse_leftbias :
  forall A a f,
  morelse A a (a >>= compose (unit _) f) = a.
Axiom morelse_naturality :
  forall A B (f : A -> B) a b,
  morelse _ (map f a) (map f b) = map f (morelse _ a b).


(* MonadOr / MonadZip Distributivity *)
Axiom joinad_distributivity :
  forall A B a b1 b2,
  morelse _ (mzip A B a b1) (mzip _ _ a b2) = mzip _ _ a (morelse _ b1 b2).


(* MonadDuplicate laws (cancellation, lifting, nesting, zero, unit, naturality) *)

Axiom mdup_cancel : (* right identity *)
  forall A a,
  (mduplicate A a) >>= id = a.
Axiom mdup_unit : (* unit identity *)
  forall A B a, forall (f : J A -> J B),
  mduplicate _ (unit _ a) >>= f = f (unit _ a).
Axiom mdup_zero : (* zero identity *)
  forall A B, forall (f : J A -> J B),
  mduplicate _ (mzero _) >>= f = f (mzero _).

Axiom mdup_lifting : (* lifting over mzip *)
  forall A B C (m : J A) (n : J B) (f : J A -> J C),
  mzip _ _ ((mduplicate _ m) >>= f) n = (mduplicate _ m) >>= (fun m => mzip _ _ (f m) n).
Axiom mdup_nesting : (* composition *)
  forall A B C (m : J A) (k : J A -> J B) (l : J B -> J C),
  mduplicate _ ((mduplicate _ m) >>= k) >>= l = mduplicate _ m >>= (compose l k).
Axiom mdup_symmetry : (* required by reordering *)
  forall A B C (m : J A) (n : J B) (f : J A -> J B -> J C),
  mduplicate _ m >>= (fun x => mduplicate _ n >>= (f x)) 
= mduplicate _ n >>= (fun y => mduplicate _ m >>= (flip f y)).

Axiom mdup_naturality :
  forall A B C m, forall (f : A -> B), forall (g : J B -> J C),
  (mduplicate _ (map f m)) >>= g = mduplicate _ m >>= (compose g (map f)).

(* ------------------------------------------------------------------------- *)
(* Trivial helper lemmas                                                     *)
(* ------------------------------------------------------------------------- *)

Lemma mduplicate_eq : 
  forall A f g,
  f = g -> mduplicate A f = mduplicate _ g.
Proof. intros until 0; intros ->; trivial. Qed.

Lemma mzip_eq :
  forall A B a1 a2 b1 b2,
  a1 = a2 /\ b1 = b2 -> mzip A B a1 b1 = mzip _ _ a2 b2.
Proof. intros until 0; intros [-> ->]; trivial. Qed.

Lemma morelse_eq : 
  forall A a1 b1 a2 b2,
  a1 = a2 /\ b1 = b2 -> morelse A a1 b1 = morelse _ a2 b2.
Proof. intros until 0; intros [ -> ->]; trivial. Qed.

Lemma bind_eq : 
  forall (A B : Type), forall a1 a2, forall (f1 f2 : A -> J B),
  a1 = a2 /\ f1 = f2 -> a1 >>= f1 = a2 >>= f2.
Proof. intros until 0; intros [-> ->]; trivial. Qed.

Lemma unit_eq : 
  forall A a1 a2,
  a1 = a2 -> unit A a1 = unit A a2.
Proof. intros until 0; intros ->; trivial. Qed.

(* ------------------------------------------------------------------------- *)
(* (1) Binding equivalence                                                   *)
(* ------------------------------------------------------------------------- *)

Lemma binding_equivalence :
  forall (A B: Type),
  forall (m : J A), forall (f : A -> J B),
  (*    
  docase m of v -> return v == do v <- m; return v 
  *)
  (mduplicate _ m) >>= (fun m' =>
    (m' >>= (fun v => unit _ (unit _ v))) >>= id)
  = m >>= (fun v => (unit _ v)).
Proof.
  intros A B m f.
  
  assert (unit_join : forall C, forall (x : J C), 
         (x >>= (fun v => unit _ (unit _ v))) >>= id
        = x >>= (fun v => (unit _ v))).
    intros.
    rewrite mbind_assoc.
    apply bind_eq.
    split; trivial.
    apply functional_extensionality.
    intro.
    rewrite mbind_unit_left.
    trivial.

  assert (nested:
    mduplicate A m >>= (fun m' : J A => (m' >>= (fun v : A => unit _ (unit _ v))) >>= id) 
  = mduplicate A m >>= (fun m' : J A =>  m' >>= (fun v : A => unit _ v))).
      apply bind_eq.
      split; trivial.
      apply functional_extensionality, unit_join.

  rewrite nested.
  rewrite <- mbind_assoc.
  rewrite mdup_cancel.
  trivial.
Qed.

(* ------------------------------------------------------------------------- *)
(* (3) Clause ordering                                                       *)
(* ------------------------------------------------------------------------- *)

Lemma mbind_naturality : 
  forall A B C a, forall (f : A -> B), forall (g : B -> J C),
  a >>= (compose g f) = (map f a) >>= g.
Proof.
  intros.
  unfold map.
  rewrite mbind_assoc.
  unfold compose.
  apply bind_eq; split; [trivial|apply functional_extensionality; intros].
  rewrite mbind_unit_left.
  reflexivity.
Qed.

Lemma map_naturality :
  forall A B C a (f : B -> C) (g : A -> B),
  map (compose f g) a = map f (map g a).
Proof. 
  intros; unfold map. 
  replace ((a >>= compose (unit B) g) >>= compose (unit C) f) with
          (map g a >>= compose (unit _) f).
  rewrite <- mbind_naturality. 
  apply bind_eq; split; [trivial|unfold compose;trivial].
  unfold map; trivial.
Qed.

(* equivalent, but more verbose version using parametricity *)
Lemma morelse_leftbias_map :
  forall A B a, forall (f g : A -> B),
  morelse B (a >>= compose (unit _) f) 
            (a >>= compose (unit _) g) 
          = (a >>= compose (unit _) f).
Proof.
  intros.
  replace (a >>= compose (unit _) f) with (map f a).
  replace (a >>= compose (unit _) g) with (map g a).
  
  assert(transform:
      map (fun q => fst q) 
       (morelse _                                 (map (fun arg => (f arg, arg)) a)
         (map (fun tup => (g (snd tup), snd tup)) (map (fun arg => (f arg, arg)) a)))
    = morelse _ (map f a) (map g a) ).
    rewrite <- morelse_naturality.
    apply morelse_eq; split.
    rewrite <- map_naturality.
    unfold compose, fst, map.
    apply bind_eq; split; [trivial|unfold compose; apply functional_extensionality; intros; trivial].
    rewrite <- map_naturality.
    rewrite <- map_naturality.
    unfold compose, fst, snd, map.
    apply bind_eq; split; [trivial|unfold compose; apply functional_extensionality; intros; trivial].

  rewrite <- transform; clear transform.
  unfold map.
  rewrite morelse_leftbias.

  assert(maps:
    map (fun q => fst q) (map (fun arg => (f arg, arg)) a) =
    (a >>= compose (unit _) (fun arg => (f arg, arg))) >>= compose (unit _) (fun q => fst q)).
    unfold map; reflexivity.
  rewrite <- maps; clear maps.
  rewrite <- map_naturality.
  unfold map, compose.
  apply bind_eq; split; [trivial|apply functional_extensionality; intros; trivial].
  unfold map; trivial.
  unfold map; trivial.
Qed.

Lemma clause_ordering :
  forall (A B: Type),
  forall (m : J A), forall (f g : A -> J B),
  (*    
  docase m of v -> f v; v -> g v == docase m of v -> f v 
  *)
  (mduplicate _ m) >>= (fun m' =>
    morelse _
      (m' >>= (fun v => unit _ (f v))) 
      (m' >>= (fun v => unit _ (g v)))
    >>= id) = 
  (mduplicate _ m) >>= (fun m' =>
    (m' >>= (fun v => unit _ (f v)))
    >>= id).
Proof.
  intros.
  apply bind_eq.
  split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].
  apply bind_eq.
  split; [ | trivial ].
  apply morelse_leftbias_map.
Qed.

(* ------------------------------------------------------------------------- *)
(* (4) Alternative noniterference                                            *)
(* ------------------------------------------------------------------------- *)

Lemma alternative_noninterference_left :
  forall (A B: Type),
  forall (m : J A), forall (f : A -> J B),
  (*    
     docase m of v -> f v; * -> g v
  == docase m of v -> f v 
  *)
  (mduplicate _ m) >>= (fun m' =>
    morelse _
      (m' >>= (fun v => unit _ (f v))) 
      (m' >>= (fun v => mzero _))
    >>= id) = 
  (mduplicate _ m) >>= (fun m' =>
    (m' >>= (fun v => unit _ (f v)))
    >>= id).
Proof.
  intros; apply bind_eq; split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].
  apply bind_eq; split; [ | trivial ].
  rewrite mbind_zero_right.
  rewrite morelse_unit_right.
  reflexivity.
Qed.

Lemma alternative_noninterference_right :
  forall (A B: Type),
  forall (m : J A), forall (f : A -> J B),
  (*    
     docase m of * -> g v; v -> f v
  == docase m of v -> f v 
  *)
  (mduplicate _ m) >>= (fun m' =>
    morelse _
      (m' >>= (fun v => mzero _))
      (m' >>= (fun v => unit _ (f v))) 
    >>= id) = 
  (mduplicate _ m) >>= (fun m' =>
    (m' >>= (fun v => unit _ (f v)))
    >>= id).
Proof.
  intros; apply bind_eq; split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].
  apply bind_eq; split; [ | trivial ].
  rewrite mbind_zero_right.
  rewrite morelse_unit_left.
  reflexivity.
Qed.

(* ------------------------------------------------------------------------- *)
(* (5) Argument noniterference                                               *)
(* ------------------------------------------------------------------------- *)

Lemma argument_noninterference_left :
  forall A B C, forall (m : J A), forall (f : A -> J C), forall g,
  (*    
     docase m, mzero of v, ? -> f v; v1, v2 -> g v1 v2
  == docase m of v -> f v 
  *)
  (mduplicate _ m) >>= (fun m' =>
  (mduplicate _ (mzero B)) >>= (fun mz =>
    morelse _
      (m' >>= (fun v => unit _ (f v))) 
      (mzip _ _ m' mz >>= (fun arg => unit _ (g arg)))
    >>= id)) = 
  (mduplicate _ m) >>= (fun m' =>
    (m' >>= (fun v => unit _ (f v)))
    >>= id).
Proof.
  intros; apply bind_eq; split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].
  rewrite mdup_zero.
  apply bind_eq; split; [ | trivial ].
  rewrite mzip_zero, mbind_zero_left, morelse_unit_right.
  reflexivity.
Qed.
  
Lemma argument_noninterference_right :
  forall A B C, forall (m : J A), forall (f : A -> J C), forall g,
  (*    
     docase mzero, m of v1, v2 -> g v1 v2; v, ? -> f v
  == docase m of v -> f v 
  *)
  (mduplicate _ (mzero B)) >>= (fun mz =>
  (mduplicate _ m) >>= (fun m' =>
    morelse _
      (mzip _ _ m' mz >>= (fun arg => unit _ (g arg)))
      (m' >>= (fun v => unit _ (f v))) 
    >>= id)) = 
  (mduplicate _ m) >>= (fun m' =>
    (m' >>= (fun v => unit _ (f v)))
    >>= id).
Proof.
  intros.
  rewrite mdup_zero.
  apply bind_eq; split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].
  apply bind_eq; split; [ | trivial ].
  rewrite mzip_zero, mbind_zero_left, morelse_unit_left.
  reflexivity.
Qed.

(* ------------------------------------------------------------------------- *)
(* (6) Matching units                                                        *)
(* ------------------------------------------------------------------------- *)

Lemma matching_units :
  forall A B C a b, forall (f : (A * B) -> J C),
  (*    
     docase (return a), (return b) of v1, v2 -> f v1 v2 
  == case a, b of v1, v2 -> f v1 v2  
  *)
  (mduplicate _ (unit A a)) >>= (fun ma =>
  (mduplicate _ (unit B b)) >>= (fun mb =>
    (mzip _ _ ma mb >>= (fun arg => unit _ (f arg)))
    >>= id)) = 
  f (a, b).
Proof.
  intros.
  rewrite mdup_unit, mdup_unit, mzip_product.
  rewrite mbind_unit_left, mbind_unit_left.
  trivial.
Qed.

(* ------------------------------------------------------------------------- *)
(* (7) Matching images                                                       *)
(* ------------------------------------------------------------------------- *)

Lemma munit_naturality : 
  forall A B f a,
  unit B (f a) = map f (unit A a).
Proof. intros; unfold map; rewrite mbind_unit_left; unfold compose; trivial. Qed.

Lemma matching_images :
  forall A B C D R a b, forall (h : (B * D) -> J R), forall (f : A -> B), forall (g : C -> D),
  (*    
     docase (return a), (return b) of v1, v2 -> f v1 v2 
  == case a, b of v1, v2 -> f v1 v2  
  *)

  (mduplicate B (map f a)) >>= (fun ma =>
  (mduplicate D (map g b)) >>= (fun mb =>
    (mzip _ _ ma mb >>= (fun arg => unit _ (h (fst arg, snd arg))))
    >>= id)) = 
  (mduplicate _ a) >>= (fun ma =>
  (mduplicate _ b) >>= (fun mb =>
    (mzip _ _ ma mb >>= (fun arg => unit _ (h (f (fst arg), g (snd arg)))))
    >>= id)).
Proof.
  intros.
  rewrite mdup_naturality.
  apply bind_eq; split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].
  unfold compose.
  rewrite mdup_naturality.
  apply bind_eq; split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].
  unfold compose.
  apply bind_eq; split; [ | trivial ]. 
  rewrite mzip_naturality.

  assert(body:
      (fun arg => unit (J R) (h (f (fst arg), g (snd arg)))) =
      (fun arg => map h (unit _ (f (fst arg), g (snd arg))))).
    apply functional_extensionality.
    intros; rewrite munit_naturality; trivial.
  rewrite body; clear body.

  Check mbind_naturality.

  assert(body:
      (fun arg : A * C => map h (unit (B * D) (f (fst arg), g (snd arg)))) =
      compose (compose (map h) (unit _)) (fun arg => (f (fst arg), g (snd arg)))).
    unfold compose; apply functional_extensionality; trivial.
  rewrite body; clear body.
  rewrite mbind_naturality.
  unfold compose.
  apply bind_eq.
  split.

  unfold map.
  apply bind_eq.
  split; [trivial | trivial].

  apply functional_extensionality.
  intros.
  rewrite munit_naturality.
  unfold map. 
  apply bind_eq.
  split; [ apply unit_eq; rewrite surjective_pairing; trivial | trivial ].
Qed.

(* ------------------------------------------------------------------------- *)
(* (8) Matching duplicate                                                    *)
(* ------------------------------------------------------------------------- *)

Lemma matching_duplicate :
  forall A B a, forall (f : A * A -> J B), 
  (*    
     docase a, a of u, v -> f (u, v)
  == docase a of v -> f (v, v)

     (modulo additional duplication)
  *)
  (*(mduplicate A a) >>= (fun ma1 =>*)
  (mduplicate A a) >>= (fun ma =>
    (mzip _ _ ma ma >>= (fun arg => unit _ (f arg)))
    >>= id) = 
  (mduplicate _ a) >>= (fun ma =>
    (ma >>= (fun v => unit _ (f (v, v))))
    >>= id).
Proof.
  intros; apply bind_eq; split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].
  apply bind_eq; split; [ | trivial ].
  rewrite mzip_duplication; rewrite <- mbind_naturality.
  unfold compose; unfold dup;
  trivial.
Qed.  

(* ------------------------------------------------------------------------- *)
(* (9) Flattening                                                            *)
(* ------------------------------------------------------------------------- *)

Lemma mdup_lifting_sym :
  forall A B C (m : J A) (n : J B) (f : J A -> J C),
  mzip _ _ n ((mduplicate _ m) >>= f) = (mduplicate _ m) >>= (fun m => mzip _ _ n (f m)).
Proof.
  intros.
  rewrite mzip_sym.
  rewrite mdup_lifting.
  rewrite mbind_assoc.
  apply bind_eq; split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].
  rewrite <- mzip_sym.
  trivial.
Qed.

Lemma mzip_naturality_bind :
  forall A B C D a b, forall (f : A -> B), forall (g : C -> D),
  mzip _ _ (map f a) (map g b) =
  (mzip _ _ a b) >>= compose (unit _) (fun x => (f (fst x), g (snd x))).
  intros.
  rewrite mzip_naturality.
  unfold map; trivial.
Qed.

Axiom mzip_natural_app :  
  forall A B f a,
  (mzip (A -> B) A f a) >>= (fun arg => unit _ ((fst arg) (snd arg))) =
  (mzip (A -> B) A f a) >>= (fun arg => unit _ ((fst arg) (snd arg))).

Lemma flattening :
   forall A B C D E (m : J A) (n1 : J B) (n2 : J C),
   forall (f : A -> D) (g1 : B -> E) (g2 : C -> E),
  (*    
     docase m, n1, n2 of v, v1, ? -> return (f v, g1 v1); v, ?, v2 -> return (f v, g2 v2)
  == docase m, 
      (docase n1, n2 of 
         v1, ? -> return (g1 v1); 
         ?, v2 -> return (g2 v2)) of v, a -> (f v, a)
  *)
  (mduplicate A m) >>= (fun m' =>
  (mduplicate B n1) >>= (fun n1' =>
  (mduplicate C n2) >>= (fun n2' =>
    morelse _
      ((mzip _ _ m' n1') >>= (fun arg => unit _ (unit _ (f (fst arg), g1 (snd arg)))))
      ((mzip _ _ m' n2') >>= (fun arg => unit _ (unit _ (f (fst arg), g2 (snd arg)))))
    >>= id))) = 

  (mduplicate _ m) >>= (fun m' =>
  (mduplicate _
   ( (mduplicate _ n1) >>= (fun n1' =>
     (mduplicate _ n2) >>= (fun n2' =>
        morelse _
          (n1' >>= (fun v1 => unit _ (unit _ (g1 v1)) ))
          (n2' >>= (fun v2 => unit _ (unit _ (g2 v2)) ))
        >>= id)) ) >>= (fun ma =>
    ((mzip _ _ m' ma) >>= (fun arg => unit _ (unit _ (f (fst arg), snd arg))))
    >>= id))).
Proof.
  intros.
  intros; apply bind_eq; split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].
  
  Check mdup_nesting.
  rewrite mdup_nesting; unfold compose.
  Check mdup_lifting.

  Check mzip_sym.

  assert(body:
      (fun x0 => (mzip _ _ x (mduplicate _ n2 >>= (fun n2' =>
         morelse _
           (x0 >>= (fun v1 => unit _ (unit _ (g1 v1))))
           (n2' >>= (fun v2 => unit _ (unit _ (g2 v2)))) >>=
         id)) >>= (fun arg => unit _ (unit _ (f (fst arg), snd arg)))) >>= id) = 
      (fun x0 => ((mduplicate C n2 >>= (fun n2' => mzip _ _ x
         (morelse _
           (x0 >>= (fun v1 => unit _ (unit _ (g1 v1))))
           (n2' >>= (fun v2 => unit _ (unit _ (g2 v2)))) >>=
         id))) >>= (fun arg => unit _ (unit _ (f (fst arg), snd arg)))) >>= id) ).
    apply functional_extensionality; intros.
    rewrite mdup_lifting_sym; trivial.
  rewrite body; clear body.

  intros; apply bind_eq; split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].
  rewrite mbind_assoc; rewrite mbind_assoc.
  intros; apply bind_eq; split; [ apply mduplicate_eq; trivial | apply functional_extensionality; intro ].

  assert(left_side:
      morelse _
        (mzip A B x x0 >>= (fun arg => unit _ (unit _ (f (fst arg), g1 (snd arg)))))
        (mzip A C x x1 >>= (fun arg => unit _ (unit _ (f (fst arg), g2 (snd arg))))) =
      map (unit _)
        (morelse _ (mzip D E (map f x) (map g1 x0))
                   (mzip D E (map f x) (map g2 x1))) ).

    assert(one_part: forall B x' g,
          (mzip A B x x' >>= (fun arg => unit _ (unit (D * E) (f (fst arg), g (snd arg))))) = 
          (map (fun arg => (unit _ (f (fst arg), g (snd arg)))) (mzip A B x x'))).
       intros; unfold map; unfold compose.
       apply bind_eq; split; [ trivial | apply functional_extensionality; trivial ].

    rewrite one_part, one_part; clear one_part.

    replace (fun arg => unit _ (f (fst arg), g1 (snd arg))) 
       with (compose (unit _) (fun arg => (f (fst arg), g1 (snd arg)))).
    replace (fun arg => unit _ (f (fst arg), g2 (snd arg))) 
       with (compose (unit _) (fun arg => (f (fst arg), g2 (snd arg)))).
    rewrite map_naturality, map_naturality, morelse_naturality.
    rewrite <- mzip_naturality.
    rewrite <- mzip_naturality.
    trivial.
    unfold compose; apply functional_extensionality; trivial.
    unfold compose; apply functional_extensionality; trivial.

  rewrite left_side; clear left_side.
  rewrite joinad_distributivity.

  Check mbind_assoc.
  rewrite <- mbind_assoc.
  apply bind_eq; split; [|trivial].

  Check mzip_naturality.
  Check mbind_naturality.

  assert(rewrite_right:
       (fun x2 : A * E => unit _ (unit (D * E) (f (fst x2), snd x2)))
     = (compose (compose (unit _) (unit _)) (fun x2 => (f (fst x2), id (snd x2))))).
    unfold compose; trivial.
  rewrite rewrite_right; clear rewrite_right.

  Check mbind_naturality.
  rewrite mbind_naturality.
  unfold map at 1.
  apply bind_eq; split; [|trivial].
  rewrite <- mzip_naturality.
  apply mzip_eq; split; [trivial|].

  Check morelse_naturality.
  unfold map at 3.
  rewrite mbind_assoc.

  unfold id, compose.

  assert(right_id:
      (fun x2 : J E => x2 >>= (fun x3 : E => unit E x3))
    = (fun x2 : J E => x2)).
    apply functional_extensionality; intros.
    rewrite <- eta_expansion_dep.
    rewrite mbind_unit_right; trivial.
  rewrite right_id; clear right_id.
 
  assert(H: (fun x2 : J E => x2) = id).
  unfold id; trivial. rewrite H; clear H.

  Check morelse_naturality.
  
  assert(foo:forall B (x : J B) (g : B -> E),
      (x >>= (fun v : B => unit (J E) (unit E (g v))))
      = (map (compose (unit _) g) x)).
    unfold map; unfold compose; trivial.
  rewrite foo.
  rewrite foo.

  rewrite map_naturality.
  rewrite map_naturality.

  Check morelse_naturality.
  rewrite morelse_naturality.
  unfold map at 3.

  rewrite mbind_assoc.
  unfold compose.
  
  assert(bar:
    (fun x2 : E => unit (J E) (unit E x2) >>= id) =
    (fun x2 : E => unit _ x2)).
    apply functional_extensionality; intro.
    rewrite mbind_unit_left; trivial.
  rewrite bar.

  assert(H: (fun x2 : E => unit E x2) = (unit E)).
    rewrite <- eta_expansion_dep; trivial.
  rewrite H.
  rewrite mbind_unit_right.
  unfold map; unfold compose.
  reflexivity.
Qed.
