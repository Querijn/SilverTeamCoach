# Silver Team Coach

### Description
Manage your own team of champions based on your own Champion Mastery in League of Legends! This game has been made for the API Challenge of April 2016, competing for the "Entertainment" category.
##### Try out the demo at: [http://querijn.codes/silver/team/coach/][SilverTeamCoach] 
###
##### Why should you play Silver Team Coach? [Watch the video here!][PlaySilverTeamCoach]

###
### How to set up
[Watch the video on how to set up Silver Team Coach here!][Videoinstructions]

In order to play the game, you will need to do the following steps:
- Go to [Silver Team Coach on Github][GithubSilverTeamCoach] and download the ZIP (Download ZIP). 
- [Download UWamp][UWamp], unpack it in ```SilverTeamCoach/UWamp/``` and run the wamp server
- Open the Silver Team Coach project in Unity and build it using Build Settings and select WebGL (if WebGL is not available, please download and install it from the Unity website)
- Select the folder ```SilverTeamCoach/UWamp/www``` as build folder
- Delete "Index.html" from ```SilverTeamCoach/UWamp/www```.
- While it's building, go to the [Riot Developer's site][RitoDev], and register for a key. Put that key in ```SilverTeamCoach/UWamp/www/key/key_file```. (create that file if it does not exist)
- When Unity is done building, go to the wamp server and select "Browser www" or simply go to localhost in your web browser
- Play the game!

### Requirements
- PHP version 5.6
- MySQL
- Unity with WebGL output

### Game features
- Log in with your own League of Legends account, and use it to buy Champions in game.
- Set up your own League of Legends team to beat other players!
- Level up your Champion Mastery in League of Legends to have your champions in Silver Team Coach perform better.
- Fight against bot teams generated by us and see how well you're doing (and see if you recognize the bot players).

### Post Mortem
We made this game for the API Challenge of April 2016. We decided it would be fun to be able to see how well the champions gamers often play would function as a team. We initially started working on the menu content, making sure all of that was done before we started on the actual game.

We divided who would do what based on experience, which meant Querijn was going to do the more difficult programming parts while Liane worked on the easier programming tasks and the layout and design. 

Although the time limit was well-known, we did something a lot of game designers do, and that's overscoping. We basically overscoped everything. We wanted it to be a game in which you pitted your team of champions against another in a kind of Legends of League of Legends game and became stronger and more efficient the more you played. This is why we ran into a little bit of trouble at the end, still fixing bugs from the actual game. Although the menu was done and pretty, it probably would have been a better idea to focus on the game part first instead of the stuff leading up to it. Aside from that, we managed to create a slightly simpler version of the initial plan that still represented most of our wishes of what the game should be.

Despite everything we created at a fun game together which we hope a lot of people will enjoy!


### External Libraries
- [Kevin Ohashi's PHP Riot API][PHPRiotAPI]
- Unity 5.3.4f1

### Development

You can contribute by testing and giving feedback using the [Issues tab of our Github page!][Issues]. Currently, I'm looking into doing the following:
- HTTP Requests on other platforms, and passing cookies through those requests, so people can run it on their phones.
- A rework of the server side. 
- More in-depth gameplay and strategy.
- Alternative login methods.

### Support

- **I try to run uWamp but it gives me an error that there's already running on port X.**
    - Then there's another program already running. Go to your localhost (http://localhost/). If there is no error page, but it's blank, there's a good chance it's Skype. In the case of Teamviewer, it will say that Teamviewer is running. Close those applications and try again.
- **How do I run this on a server?**
    - First of all, you will have to change your ```UWamp/www/settings.php``` file to fit your info. The url setting needs to point to your root folder of where you're putting it (wherever the settings.php will be located on your server). After that, go to ```Assets/Scripts/Settings.cs```, in which there will be a string for host, change that string to the exact same. And after that, you should be good to go!
- **I am getting NOT_LOGGED_IN errors!**
    - Did you delete index.html after building? Unity generated it, and it needs to go. Otherwise, if you're running it from Unity, you need to set it up with the testing settings. See the question 'Can I run the game in the Unity editor?' below.
- **I am getting ACCESS_DENIED or SERVER_ERROR errors!**
    - This is probably on Riot Games' end. Either your key is incorrect, banned, disabled, or the servers are down. 
- **I am getting an error that I am not logged in when I am running the game. 'Did you set up from source correctly?'**
    - See the next question. This is here for those google occassions.
- **Can I run the game in the Unity editor?**
    - Yes, you can! There are some limitations: it means you're the only one that could play on that server. That is because of the lack of cookies in Unity; thus disallowing us to remember with each time you contact the server who you are. Go to settings.php in the uwamp/www folder, there's a line that start with ```$settings['testing'] = false```, change that to ```settings['testing'] = true;```. One of the following lines should be  along the lines of ```$settings["testing_account"] = 0;```, change that zero to your account number, which you can find on lolking in the url. For instance, my lolking link is www.lolking.net/summoner/euw/22929336, so my account number is 22929336. For me, it should be ```$settings["testing_account"] = 22929336;```. Make sure all the lines end with ```;```! Save the file and run your game in the Unity editor.
- **The tutorial asks me to build in WebGL but it's not in the list of build methods!**
    - Go to the Unity website and download Unity again. Make sure it's a build that supports multiple scenes! We've made it in 5.3.4f1, so that version should be fine, just go with the latest! In the installer, make sure you select the WebGL option.
- **Can I use XXZVDDAmp 3.462.23 Master Edition instead of UWamp?**
    - Sure, but don't expect me to help you if it doesn't work.
- **How do I change the prefix of the tables? I can't have multiple databases on my server!**
    - ```$settings["mysql_prefix"]``` is the setting in ```UWamp/www/settings.php``` that you're looking for, make sure you're terminating it with an underscore if you need it to. if you want the ```champions``` table to be called ```stc_champions```, put ```"stc_";``` as your prefix.

### Questions

- ***If I compile for WebGL, should I tick Development Build?***
    - Not necessary if you're going to just play, if you're going to debug, I suggest increasing the memory, or using the testing settings mentioned in the 'Can I run the game in the Unity editor?' above.
- **Does UWamp have to run to play the game locally?**
    - Unfortunately, yes. Due to the restrictions of the Riot API's agreement, which included that we can't put our API key in our compiled application, we couldn't call to the League of Legends stuff without backfire from the company! There are other options though, you can play at http://querijn.codes/silver/team/coach/ as well, or put the game on your own server, if you have one. Just drop everything from the uwamp/www folder onto your AMP setup and follow the steps in the setup instructions in the support section of this document!
- **Can I compile this for anything but WebGL?**
    - Unfortunately due to the way WebGL makes its requests, you cannot (unless you can use a way to save/load cookies!). HTTP Requests are used a lot in the game, and running this in the browser using the WebGL build method translate those requests into XMLHttpRequests, with Javascript, which sends the cookie along, giving us the ability to work with Unity.
- **Why Unity?**
    - One of the developers, Silkspectred, is not a weathered hardcore coder, but she loves to help. One of the languages she currently can work with is C#, which is one of Unity's supported languages. Querijn, on the other hand, is very experienced with Unity, and can therefore manifest a lot of work, which made us able to do a lot of work.
- **Why PHP?**
    - It's the only server-sided scripting language Querijn knows. He has worked with Kevin Ohashi's PHP library for the Riot API before, and decided it was the way to go. The downside is the fact Liane does not know this language, and thus Querijn had to work on the server-side solo. In hindsight, I should've gone for an active connection with a C# server, but Querijn wanted something he could upload to his hosting.
- **So why not Ruby or Python? It's a lo-**
    - ![me irl][me_irl]

### Special Thanks
- James-Richard Eckert
- David Chow
- Kevin Ohashi
- Abdullah Miraj
- James Glenn

   [me_irl]: <http://irule.at/images/me_irl.jpg>
   [PHPRiotAPI]: <https://github.com/kevinohashi/php-riot-api>
   [RitoDev]: <https://developer.riotgames.com>
   [Issues]: <https://github.com/Querijn/SilverTeamCoach/issues>
   [GithubSilverTeamCoach]: <https://github.com/Querijn/SilverTeamCoach>
   [UWamp]: <http://www.uwamp.com/en/>
   [Videoinstructions]: <https://www.youtube.com/watch?v=vh8E40t_rSU&feature=youtu.be>
   [SilverTeamCoach]: <http://querijn.codes/silver/team/coach/>
   [PlaySilverTeamCoach]: <https://youtu.be/qbKkC_ndMtA>