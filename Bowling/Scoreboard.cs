using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Bowling
{

    public class Scoreboard
    {

        public int numFrames { get; private set; }
        public Dictionary<string, List<List<int>>> Frames { get; private set; }
        public Dictionary<string, List<int>> Scores { get;  private set; }

        public Scoreboard(int numFrames = 10)
        {
            this.numFrames = numFrames;
            Frames = new Dictionary<string, List<List<int>>>();
            Scores = new Dictionary<string, List<int>>();
        }

        public Scoreboard(List<string> players, int numFrames = 10)
        {
            this.numFrames = numFrames;
            Frames = new Dictionary<string, List<List<int>>>();
            Scores = new Dictionary<string, List<int>>();
            foreach(string playerName in players)
            {
                AddPlayer(playerName);
            }
        }

        /// <summary>
        /// Creates a new set of frames and scores for a given player name
        /// </summary>
        /// <param name="playerName"></param>
        public void AddPlayer(string playerName)
        {
            try
            {
                Frames.Add(playerName, new List<List<int>>());
            }
            catch (ArgumentException)
            {
                throw new DuplicatePlayerException();
            }
            for(int i=0; i<numFrames; i++)
            {
                Frames[playerName].Add(new List<int>());
            }
            Scores[playerName] = new int[numFrames].ToList();
        }

        /// <summary>
        /// Records a new roll for a given player at the current frame and the number of pins knocked
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="pins"></param>
        public void Roll(string playerName, int pins, int frame)
        {
            int frameIndex = ValidFrame(frame);
            if (pins < 0 || pins > 10)
            {
                throw new NumPinsException("Attempt to roll fewer than 0 or more than 10 pins");
            }

            List<int> currFrame = Frames[playerName][frameIndex];
            currFrame.Add(pins);

            if (frame < numFrames)
            {
                if (IsStrike(currFrame) && currFrame.Count > 1)
                {
                    throw new InvalidRollException($"Attempt to roll ball in the same frame after a strike");
                }
                else if (currFrame.Count > 2)
                {
                    throw new InvalidRollException($"Attempt to record too many rolls for {playerName} at frame {frame}");
                }
                else if (currFrame.Count > 1 && currFrame[0] + pins > 10)
                {
                    throw new NumPinsException($"Sum of pins exceeds 10");
                }
            }
            else
            {
                if (IsStrike(currFrame) || IsSpare(currFrame))
                {
                    if (currFrame.Count > 3)
                    {
                        throw new InvalidRollException($"Exceeding number of bonus rolls for final frame strike or spare");
                    }
                }
                else if (currFrame.Count > 2)
                {
                    throw new InvalidRollException($"Attempt to record too many rolls for {playerName} at frame {frame}");
                }
                else if (currFrame.Count > 1 && currFrame[0] + pins > 10)
                {
                    throw new NumPinsException($"Sum of pins exceeds 10");
                }
            }

            Frames[playerName][frameIndex] = currFrame;
            
        }

        public bool IsStrike(List<int> frame)
        {
            return frame[0].Equals(10);
        }

        public bool IsSpare(List<int> frame)
        {
            return frame.Take(2).Sum().Equals(10);
        }

        private int ValidFrame(int frame)
        {
            if (frame < 0 || frame > numFrames)
            {
                throw new IndexOutOfRangeException("Attempting to score an invalid frame number");
            }
            return frame-1;
        }

        /// <summary>
        /// Calculates the total score for a given player and frame
        /// Can be called for strike or spare frames before their bonus rolls 
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="frameIndex"></param>
        /// <returns>Score value for the frame</returns>
        public int ScoreFrame(string playerName, int frameNum)
        {
            int frameIndex = ValidFrame(frameNum);
            List<int> currFrame = Frames[playerName][frameIndex];
            // no rolls
            if (currFrame.Count < 1)
            {
                return 0;
            }
            if (frameNum < numFrames)
            {
                
                if (IsStrike(currFrame))
                {
                    if (Frames[playerName].ElementAtOrDefault(frameIndex + 1) == null)
                    {
                        return 10;
                    }
                    // followed by a strike
                    if (IsStrike(Frames[playerName][frameIndex + 1]))
                    {
                        try
                        {
                            return 20 + Frames[playerName][frameIndex + 2][0];
                        }
                        catch (ArgumentOutOfRangeException) // Unique to the second to last frame 
                        {
                            return 20 + Frames[playerName][frameIndex + 1][1];
                        }
                    }
                    return 10 + Frames[playerName][frameIndex + 1].Sum();
                }
                if (IsSpare(currFrame))
                {
                    if (Frames[playerName].ElementAtOrDefault(frameIndex + 1) == null)
                    {
                        return 10;
                    }
                    return 10 + Frames[playerName][frameIndex + 1][0];
                }
            }

            return Frames[playerName][frameIndex].Sum();
        }

        /// <returns>JSON serialized representation of the scores</returns>
        public string ShowScoreboard()
        {
            for(int i=0; i<numFrames; i++)
            {
                foreach(string player in Frames.Keys)
                {
                    Scores[player].Add(ScoreFrame(player, i+1));
                }
            }
            return JsonSerializer.Serialize(Scores);
        }

    }
}
