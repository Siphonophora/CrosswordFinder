# 538 Riddler 

This is an attempted solution to the problem posed [on the 538 Riddler](https://fivethirtyeight.com/features/how-many-crossword-puzzles-can-you-make/)

# Approach

Find Columns
1. Find all the valid columns that can exist with at least one word and all words 3 letters or longer. 
2. Determine which columns can be placed in the first three columns OR in the middle column. 
3. Find parent/child relationships between each allowed column. A child would be identical to a parent column, but with one extra black square. While multiple valid parents exist for many columns, only one was chosen. This was done because a child column is only ever valid if a parent column is valid in the same position. Columns were grouped by 'order' which is the number of black squares. 

Find Puzzles
1. Find all puzzles which are made up of order 1 or blank columns in all positions. In the 15x15 puzzle, there are 124 million of these. 
2. Check the set of order 1 columns by starting with a blank puzzle, and 'slicing' the search space by searching left to right: Select a column for the first unfilled position from the order one columns or use a blank. Check the validity of the puzzle with this choice. If it is invalid, move on to the next choice for that column, otherwise move to the next column. Repeat for each column, finding each valid order 1 puzzle. 
3. Any time a valid order 1 puzzle is found, find every child puzzle comprised of the current order 1 columns or their order 2 children. For every valid puzzle repeat with by substituting each order 2 column for its children in turn. Repeat for all possible children.

Validate Puzzles - Puzzles are checked with three tests, run in order from fastest to slowest. Stopping after the first failure. 
1. Every row must be in the valid column list, and must be valid for its position. 
2. No cheater blocks 'Searching for any L shaped sets of 3 blocks'. This relies on the above rule to eliminate cheaters like a single black in the corner. 
3. White space much be contiguous. 

# Optimization

This solution searches only for puzzles without cheater squares (see the prompt). There are several stages used to reduce the number of puzzles that are checked. 
* For the 15x15, given the rotational symmetry, there are 113 squares to fill or about 10^35 choices. 
* Of the 2^ 15 = 32768 possible columns, only 797 are valid. Choosing 8 of these gives 10^23 choices.
* Not all columns are valid in all positions, so there are only 26^3 * 797^4 * 33 ~ 10^17 options to consider. 
* If we consider only the possible order 1 puzzles (columns with no more than 1 black) as a starting point, there are about 124 million to start with. Of these, we can skip more than 90% through the 'slices' taken of the possible order 1 puzzle space. 
* Iterating through the child columns prevents checking anything twice and allows stopping as soon as an invalid puzzle is found.

# Results 
TBD
