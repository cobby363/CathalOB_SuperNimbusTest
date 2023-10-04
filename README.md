# Cathal - SuperNimbus

## Game Idea 
The game idea that I have come up with is a Marbles racing type game. The game will be a 3D game but the camera will be restricted to top-down only. Game will be made in Unity, implementing Mirror and Nakama - going fo points 1 & 2 from the brief

## Design Choices
- Game idea overall - I feel that this is a game that I can code quite quickly on the front end
- Unity & Unity Mirror - This is a technology that I have used a small bit in the past. With Mirror, it is a feature that I still have a lot to learn in, especially with the fact that it has been about a year and a half or so since I have looked at the technology. Unity, I feel that I will be able to shake the rust on in a few hours and Mirror, I will have a fun challenge, but with some lingering familiarity.
- Nakama - This was a suggestion from the brief. I have researched each of the options and I like the look of Nakama, especially with it’s documentation and I feel that it would be a great fit for my project needs specifically. This is a totally new technology to me, so I am looking forward to delving in.

## Gameflow
The game will flow for the player as follows:
- Opens game
- UI for logging in or creating account
- Basic UI for main Menu
- Change settings/press play
- If presses play:
- Enter lobby for matchmaking
- Once 2 or more players join, the host can start the game
- Countdown to go will appear before a big GO text appears
- Upon crossing the finish line, players will see their time
- Move to the leaderboard screen where they can see the overall leaderboard and  opponents scores. If the  player has a new PB, their score on the leaderboard will be updated.
- Player will return to the homescreen, ready to start again.

## Key Features I’d like to add
- Main Menu ✅
- Login/regestration ✅
- Fun game ? - I feel I could've done a bunch more but I didn't want to spend too much time on frontend
- Leaderboards - ❎
- Multiplayer - ✅

## Issues run into along the way
- Original idea was to use beamable rather than Nakama. Through studying Beamable, I was able to get a good understanding of it, however with tutorials and some of the documentation being out of date, I eventually decided to go down another route which led me to Nakama which I was able to use much easier. Beamable is still something I would love to learn, I just feel it will take me some more time. I was planning to reach out to Sean about this issue with Beamable, however, when I was searching the alternatives, I became a bit more engrossed in Nakama and was very interested in giving this a go first.
- Original plan was to use Leaderboards, however I was unable to create a leaderboard to use in my game on Nakama. I was able to figure out adding scores into the leaderboards, I just could not manage to create the original leaderboard. It is something I may be able to do in the future, for now I was unable to do so.
- I had considered using a dedicated server for both Nakama and Mirror, I simply ran out of time. Through researching them both, I feel that it is still something I am very capable of, but for now I am only using localhost

## Things learned
- I rediscovered my love of coding for multiplayer again, despite the many headaches it can cause.
- relearning Mirror was more of a task than I had originally imagined. I had forgotten more than I'd thought and there were some new things in the technology. It was fun however learning again
- I have now a small understanding of Beamable with a good understanding of implamanting some aspects of it
- I have a good understanding of some of the basic capabilities of Nakama. I do feel that the way I set everything up can be improved to an industry standard and I am keen to do so be it with SuperNimbus or later in my future.
- extrusion tools for splines, in particular for complex shapes such as archs

## Linking back to the brief
For this project, as mentioned earlier, I focussed on section 1 & 2 of the brief. I used Unity's Mirror as the multiplayer tool, and Nakama as the backend system. The two combine noticeably in terms of the usernames being used through the scenes. Using Nakama, I was able to set up user authentication with login/creation of accounts with an email and password. When creating an account, the player can add a custom username. Any errors through this process are reported back to the player. I looked into Leaderboards and I feel I am able to add to leaderboards, however I struggled in creating the leaderboard in the firstplace. From looking into adding friends and messaging services through Nakama, I feel these are features that I can add, especially with more practice.

Mirror is used for the main Multiplayer aspects. Players are brought into a lobby where they can see their Nakama usernames, brought from there into a race, and finally brought back to a leaderboard screen which as previously mentioned, does not work. I feel that I have implamented some Mirror features well. Some maybe not that good due to having to learn some of them again near from scratch, but parts such as the lobby, I am proud of. 

The systems build off eachother with the username, and I had hoped to do so further with the leaderboards. It is something for sure that I would love to get better at as my career in games development continues.
