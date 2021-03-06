# GameLiftServerSdk C#

Unity NPM version of Game Lift Managed Server C# SDK! This repository applies the same license terms of the original version.

Original Source Location: [https://aws.amazon.com/pt/gamelift/getting-started/](https://aws.amazon.com/pt/gamelift/getting-started/).

# How to install

At package.json, add these line of code:
```json
"com.gameworkstore.gameliftsdk": "https://github.com/GameWorkstore/gamelift-server-sdk.git#4.0.2008",
```

And wait for unity to download and compile the package.

for update package for a newer version, update it's version at the of the line!
```json
"com.gameworkstore.gameliftsdk": "https://github.com/GameWorkstore/gamelift-server-sdk.git#4.0.2008",
```

## Documention
You can find the official GameLift documentation [here](https://aws.amazon.com/documentation/gamelift/).

###  Check Scripting Runtime Version setting
Make sure you set the Scripting Runtime Version to the .Net solution you're using. 
Otherwise, Unity will throw errors when importing the DLLs.  
From the Unity editor, go to:  
File->Build Settings->Player Settings. Under Other Settings->Configuration->Scripting Runtime Version

At this point, you should be ready to start playing with the SDK!

### Example code
Below is a simple MonoBehavior that showcases a simple game server initialization with GameLift.
```csharp
using UnityEngine;
using Aws.GameLift.Server;
using System.Collections.Generic;

public class GameLiftServerExampleBehavior : MonoBehaviour
{
    //This is an example of a simple integration with GameLift server SDK that will make game server processes go active on GameLift!
    public void Start()
    {
        //Identify port number (hard coded here for simplicity) the game server is listening on for player connections
        var listeningPort = 7777;

        //InitSDK will establish a local connection with GameLift's agent to enable further communication.
        var initSDKOutcome = GameLiftServerAPI.InitSDK();
        if (initSDKOutcome.Success)
        {
            ProcessParameters processParameters = new ProcessParameters(
                (gameSession) => {
                    //When a game session is created, GameLift sends an activation request to the game server and passes along the game session object containing game properties and other settings.
                    //Here is where a game server should take action based on the game session object.
                    //Once the game server is ready to receive incoming player connections, it should invoke GameLiftServerAPI.ActivateGameSession()
                    GameLiftServerAPI.ActivateGameSession();
                },
                (updateGameSession) => {
                    //When a game session is updated (e.g. by FlexMatch backfill), GameLiftsends a request to the game
                    //server containing the updated game session object.  The game server can then examine the provided
                    //matchmakerData and handle new incoming players appropriately.
                    //updateReason is the reason this update is being supplied.
                },
                () => {
                    //OnProcessTerminate callback. GameLift will invoke this callback before shutting down an instance hosting this game server.
                    //It gives this game server a chance to save its state, communicate with services, etc., before being shut down.
                    //In this case, we simply tell GameLift we are indeed going to shutdown.
                    GameLiftServerAPI.ProcessEnding();
                }, 
                () => {
                    //This is the HealthCheck callback.
                    //GameLift will invoke this callback every 60 seconds or so.
                    //Here, a game server might want to check the health of dependencies and such.
                    //Simply return true if healthy, false otherwise.
                    //The game server has 60 seconds to respond with its health status. GameLift will default to 'false' if the game server doesn't respond in time.
                    //In this case, we're always healthy!
                    return true;
                },
                listeningPort, //This game server tells GameLift that it will listen on port 7777 for incoming player connections.
                new LogParameters(new List<string>()
                {
                    //Here, the game server tells GameLift what set of files to upload when the game session ends.
                    //GameLift will upload everything specified here for the developers to fetch later.
                    "/local/game/logs/myserver.log"
                }));

            //Calling ProcessReady tells GameLift this game server is ready to receive incoming game sessions!
            var processReadyOutcome = GameLiftServerAPI.ProcessReady(processParameters);
            if (processReadyOutcome.Success)
            {
                print("ProcessReady success.");
            }
            else
            {
                print("ProcessReady failure : " + processReadyOutcome.Error.ToString());
            }
        }
        else
        {
            print("InitSDK failure : " + initSDKOutcome.Error.ToString());
        }
    }

    void OnApplicationQuit()
    {
        //Make sure to call GameLiftServerAPI.Destroy() when the application quits. This resets the local connection with GameLift's agent.
        GameLiftServerAPI.Destroy();
    }
}
```
