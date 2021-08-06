## Problem statement:
Imagine you’re tasked with writing the software for a bowling alley that shows the bowling game scoreboard.
Design entities and interfaces to model a scoreboard controller for bowling, keeping in mind the nature of the input from the physical machinery in the bowling lane, which generally reflects game and/or pin state.
The input to the scoreboard controller is the lane control hardware that reflects the pin state (i.e. how many pins are up/down).

## Your Task: 
Using your design, implement (in the programming language of your choosing) the scoring / game state display service that would compute needed data that one typically expects to see on the lane monitor during a game. 
What’s fair game? 
We want you to use your resources! You can use API documentation, and google is available but do not copy and paste a solution from the internet. Cite your sources. 


## Solution and Assumptions
1. we will define a set exceptions modeled arond the rules of bowling to ensure the app works corerctly and to enhance debugging
2. the socreboard is not concered with which pins are knocked, so we just record the number
3. number of pins knocked per roll is represented by the 'frames' dictionary with keys of player names pointing to a list of lists. the inner lists represent the  rolls in each frame. The last frame is permitted to have more than two rolls given a strike or spare.
4. we store the frame's scores in a separate dictionary of lists to implement the scoring system of bowling
5. I chose to allow the Scoreboard class to keep the Frames and Scores as mutable state for simplicity's sake, although a more functional approach would abandon this method in favor of creating new versions of these dictionaries
6. The Scoreboard itself does not keep track of the "current frame" in a game of bowling.
7. Due to the unknown or variable nature of the scoreboard's consumers, I lean towards using the exceptions for error handling rather than printing to the console and returning
8. Consumers / driving code refers to frames as 1-indexed
9. My primary concern was testability of scoreboard functionality with respect to bowling scenarios, so I wrote exhaustive unit tests
