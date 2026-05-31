# Creating your first adventure

An adventure consists of three main components

* Meta(data) - The file that contains the adventure's
    * Name
    * Starting Scene
    * Author Name (optional)

* Scenes - The rooms that the player navigates through in order to play the adventure.

* Actions/BaeScript - Files containing code to a simple programming language created for the engine to make creating adventures easier and require less cognitive overhead.

    * Makes adventures less linear, and
    * More interactive

Let's create an adventure. 

First of all, create the basic folder structure for a new adventure. Here's what it should look like

```
ADVENTURE_FOLDER/
|
|-actions/
|   |
|   |-- BaeScript/Action files go here
|
|-scenes/
|   |
|   |-- Scene files go here
|
|--meta
```

I'm going to call our adventure "Giga Chad," and it's going to be authored by "Ohio Man."

Our starting scene is going to be called "Edgington." Here's what our metadata file will end up looking like:
```bae
ADVENTURE:Giga Chad
AUTHOR:Ohio Man
STARTING:Edgington
```

After we have the metadata file all filled out, we can navigate to the scenes folder and create our starting scene "Edgington."

Create an empty text file in the scenes folder, and name it "Edgington" (You can actually name it whatever you want, but just make sure to name it something meaningful so things don't get confusing later.)

Once the new file is created, open it up in any text editor, and type this into it

```bae
ID:Edgington
TEXT:I am in the snowy town**cyan**Edgington,**white** AKA **cyan**Gooner's Sanctuary.**white**\n%ACTIONS\n\nWhat should I do? 

ACTION:1%Shout at nothing
ACTION:2%Eat snow

$1:DO%shout
$2:NAV%die
```

Here's a breakdown of what's happening *(don't let this discourage you, it's less complicated than it seems)*:
* **ID** - The scene's identifier. This is important for when you want to use NAV statements to switch the scene (more on that later)
* **TEXT** - The text that will be displayed after the PREACTIONS file finishes excecuting (more on that later.)
    * **%ACTIONS** - Inserts the list of actions/choices the player can do
    * **\*\*cyan\*\*** - Changes the following text's color to cyan until another color tag is used
    * **\n** - Used as a line break. The engine will insert an "enter," or new line in place of this character every time you use it

* **ACTION** - An individual action/choice that the player will be able to make

    * **1%Shout at Nothing** *(Looks like "1. Shout at Nothing" when the engine prints it on the screen)*
        * **1** - The prefix that will be displayed when the actions list is printed
        * **%** - The seperator that the engine uses to tell when the prefix ends
        * **Shout at Nothing** - The text that will be displayed after the prefix

* **$** - Tells the engine that a shortcut is being defined
    * **1:DO%shout**
        * **1** - The key that must be pressed to trigger the defined shortcut
        * **DO** - The command that will be excecuted when the defined shortcut is triggered *(In this case, the DO command is being excecuted. The DO command excecutes all the commands inside the BaeScript/Actions file who's ID is passed as a paramater)*
        * **%** - The seperator that tells the engine that the following characters are paramaters for the previously stated command
        * **shout** - The paramater for the previously stated command. In this case, it is the action ID "shout"