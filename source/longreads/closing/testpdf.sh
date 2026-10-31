#!/bin/bash
pdflatex -interaction=nonstopmode -output-directory=build main.tex
(cd build && BIBINPUTS="..;" bibtex main)
