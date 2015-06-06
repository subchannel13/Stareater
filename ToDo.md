# Introduction #

This page contains list of features (more like ideas) that need to be done and are not part of version being developed. Their description well be detailed as they become version requirement.


# To do list #

  * Combat (space and ground)
  * Diplomacy
  * Espionage
  * Moons and orbital habitation
  * Multi-player
  * Text size
  * Turn phases
  * Victory conditions

## Combat ##

Both ground and space combat should be simple like ground combat mechanism in Master of Orion series. In most 4x games space combat is not simple (almost like game in game) but in Zvjezdojedac it should not be complex or else project will never be finished.

## Diplomacy ##

Diplomacy interface is planed to be like one in Civilization 3+. During negotiation players can exchange anything for anything where anything is combination of technologies, colonies, permission to colonize certain planets, intelligence and treaties. Option to make ultimatum would be desirable (a demand that if rejected starts a war).

## Espionage ##

Some form of stealing technology and sabotage. Idea is not entirely developed.

## Moons and orbital habitation ##

Concept proposed mainly because of gas giants. Unlike rocky planets and asteroids, gas giants don't have colonizable surface. Instead they usually have large moons that can populated. To make gas giants less special case, moons and orbital habitation is proposed. It would be good to avoid explicit manifestation of moons and incorporate then in some simpler concept. In that manner division of planet size to surface and orbital size is proposed.

While planetary "surface" is colonizable from the beginning, colonizing orbit requires technology. Cost of living on surface is determined by atmospheric conditions and stellar radiation and can be regulated through terraforming and shielding. On the other hand orbit can't be terraformed (due the lack of terra part) and cost of living is dependant on orbital habitation technologies.

For rocky planets, "surface" is combined surface of planet and it's moons and "orbit" is orbit of both planet and moons. Asteroids in this proposal are considered purely orbital case with zero surface size. Gas giants have "surface" on moons and "orbit" around main planet body and moons. Although gas giants have considerable volume of atmosphere, to keep things simpler, it shouldn't not be taken in consideration.

By dividing planets to surface and orbit, concept of mining also has to be updated. Orbits of rocky worlds should not be place to do mining while "orbits" of asteroids and gas giants can be. In this proposal asteroids are considered to be surfaceless orbit, but that doesn't mean they can't be mined. To cover that option mineral abundance of orbit is proposed. Gas giants can have solid core that can be mined too but only after overcoming crushing atmosphere. So, to cover that option too, condition on orbital mining ability is proposed. As surface mining has mining depth that interpolates between shallow and deep mining so should orbital mining be affected by some factor of capability.

## Multi-player ##

Hot-seat and TCP/IP. Maybe something over XMPP.

## Text size ##

Current GUI looks ok on 1024x768 and even on same higher resolutions if computer user is less then 1m away from screen. Displays larger then default 14 inch laptop LCDs are usually set to resolution higher then 1024x768 also due larger diagonal some users prefer greater distance between display and their eyes. In these conditions some parts of current GUI, specifically texts, are less visible. Since GUI in this project is based on Windows forms simple decrease in resolution (like in fullscreen games) can't be applied. Proposed method is to make an option to scale text font sizes (and size of GUI elements).

## Turn phases ##

So far, there are two turn phases: phase where player manages his/hers empire and next turn calculation phase. Colonization requires extra phase, after empire management and before next turn. Space combat, bombardment and ground combat will also require separate phases and GUI.

## Victory conditions ##

Selection of victory conditions in "new game" interface, detection of these condition ingame and game over interface. For now, game can't neither be won nor lost so all three things need to be implemented. Suggestions for victory conditions
  * Domination (colonize 2/3 of "galaxy")
  * Elimination (eliminate all other players)
  * Escape (escape from Stareater, something like wonder victory)