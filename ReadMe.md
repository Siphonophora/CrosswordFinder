# 538 Riddler 

This is an attempted solution to the problem posed [on the 538 Riddler](https://fivethirtyeight.com/features/how-many-crossword-puzzles-can-you-make/)

## Prompt

> 1. They are 15-by-15.
> 2. They are rotationally symmetric — that is, if you turn the grid upside down it appears exactly the same.
> 3. All the words — that is, all the horizontal and vertical sequences of white squares — must be at least three letters long. All the letters must appear in an “across” word and a “down” word.
> 4. The grid must be entirely connected — that is, there can be no “islands” of white squares separated from the rest by black squares.
> First question: How many such crossword grids are there?
>
> Second question: Crossword constructors do well to avoid using “cheater squares,” black squares whose addition makes some words shorter but does not change the puzzle’s total word count. How many grids are there without cheater squares?
>
> Extra credit: The Sunday “New York Times” puzzle is 21-by-21. How many of those are there, with and without cheater squares?

The approach below finds puzzles without cheater squares only. For clarity, I treat any set of three blocks in an L as having a cheater square. This extends to T and + shapes. In addition, any block in the corner or sequential blocks along the edge qualify as cheaters.

## Approach

Find Columns
1. Find all the valid columns that can exist with at least one word and all words 3 letters or longer. 
2. Determine which columns can be placed in the first three columns OR in the middle column. 
3. Check each column against every other column and determine which cannot be neighbors, because they create cheater squares. 

Find Column Sets. 
1. In the 15x15 grid. Find each set of 5 columns that can be in columns 1 to 5 and find the sets of 4 columns that can populate columns 5 to 8. 
2. Check each set for words less than 3 characters. 
3. Write the column sets to disk, grouped based on column 5 which overlaps both sets.

Find Puzzles
1. For each sets of columns, test every combination of Left and Center column sets. 

Validate Puzzles - Puzzles are checked with three tests, run in order from fastest to slowest. Stopping after the first failure. 
1. Every row must be in the valid column list, and must be valid for its position. 
2. No cheater blocks 'Searching for any L shaped sets of 3 blocks'. This relies on the above rule to eliminate cheaters like a single black in the corner. 
3. White space much be contiguous. 

## Optimization

This solution searches only for puzzles without cheater squares (see the prompt). There are several stages used to reduce the number of puzzles that are checked. 
* For the 15x15, given the rotational symmetry, there are 113 squares to fill or about 10^35 choices. 
* Of the 2^ 15 = 32768 possible columns, only 797 are valid. Choosing 8 of these gives 10^23 choices.
* Not all columns are valid in all positions, so there are only 26^3 * 797^4 * 33 ~ 10^17 options to consider. 
* Creating the column sets reduced the number checked to about 10^11 and writing them to disk allowed for some parallel processing of the last step.

## Results 
TBD
