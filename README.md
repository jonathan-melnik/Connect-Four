<h1>Connect Four</h1>

<h3>By Jonathan Melnik</h3>

When I started developing the game, I opted to represent the board with a data structure that was a list of lists of discs. Each list represented a column.
When a player adds a disc the disc is added to the corresponding list.
This approach had some issues as it was hard to traverse the board and I needed to check for the length of the lists when doing it.
So I changed it to a one dimension array of discs to represent the board the whole board and doing access the items in the array using indices arithmetics.

When I started with the code that checks for a match I first thought about checking the whole board every time a disc is added. 
Then I realized that it was better to just check the matches that could have been made using the last added disc.

I was checking for straight matches(horizontal and vertical) and diagonal matches sepparately at that point. 
I thought there had to be a better way, so I reworked that code and made it into one method that checks for matches in a certain direction.

My first AI iteration was a very simple AI that just played randomly.
But I wanted to make a better AI that could defeat me.
My solution was to make the AI ask the board for a good move based on a difficulty parameter of the AI.
The good move provided by the Board would be in order: a move that makes a match, a move that prevents the opponent from making a match, a move that tries
to be the best possible move.

Finding the move that can make a match or prevent a match was easy as I could reuse the code I made for calculating if there was a match.
The difficulty came with finding a move that's optimal. 

<h3> Optimal Move</h3>

I read that this is a solved game, but I didn't look at the algorithm. My algorithm is not 100% optimal, but I played against it and it's quite competent.
In order to find a good move I check every column and for each row calculate a value to represent the potential of making a match in the future.
For each (col, row) I calculate how many discs of the same color are already placed in the board in every direction and also how many are needed to place the
ones missing to make a match in each direction.
With those two values I make a weighted sum and get the value for the potential.
I add the potential of every slot in the column and get the total potential for each column.
I then use the maximum potential for the recommended move and in case it's repeated I randomly choose one.
A potential can be positive or negative. If there's no possible match in a column I set the potential to a very low value.

Note:
I made the game so any two controllers can be assigned to each player(AI or Human), so you can have two AIs playing with each other.
