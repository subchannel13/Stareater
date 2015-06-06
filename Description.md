# Introduction #

Like many games, Zvjezdojedac is inspired by Master of Orion series but primary intention was to make a game that is not afraid of the big numbers, a 4X turn based strategy where stars can have up to 15 planes, where colony population is expressed in milliards (10^9, for those using short scale number naming system) and fleets can contain more than a million starships. In order to achieve that, many game mechanics and concepts are inspired by games outside Master of Orion series such as Galactic Civilizations, Alpha Centauri, Sword of the stars and even OGame.

# Galaxy #

Galaxy map is central user interface in 4X games. It provides the overview of the game, who owns what and fleet movement. In Zvjezdojedac, galaxy is comprised of stars and wormholes and each star can have a number of planets. Starships can only move from star to star but they can use wormholes to speed up travel. Many stars have wormholes leading to nearby stars giving the appearance of stars being connected with star lanes (like in Master of Orion III and Sword of the Stars series) but unlike many games, wormholes in Zvjezdojedac give only limited boost to interstellar travel. Speed boost is independent of drive technology and slow ships gain the most benefit form wormholes. That way early and slow interstellar drives depend on wormholes while advanced late game drives are not be constrained by wormhole network.

# Planet, colonies and star systems #

Planets are quite complex entities in this game but colony management is highly abstracted and provide a simple user interface. Colony management is inspired by Master of Orion I where each colony has a set of sliders for allocating workforce to various tasks. During initial design colonies in Zvjezdojedac had two such sliders and building queue for each one. One slider controlled resources for civil projects (infrastructure improvements, terraforming) and one for military projects (starships, planetary defenses). To reduce the micromanagement, that design was changed to only one slider and building queue per colony (planetbound projects) and a slider and building queue for whole star system (starships and starbound buildings).

# Ships #

Starships are main tool for achieving victory in space 4X games. They are used in exploration (scouting), expansion (colonization or invasion), extermination and sometimes in exploitation (mining). Some space 4X games put a lot of player's focus to space battles and allow player to design ships of he's own.

Zvjezdojedac too has designable ships, much like units in Sid Mayers Alpha Centaury. Ships are designed by choosing primary and secondary weapon/mission, a shield technology and special equipment. Missions are central part of the ship design as they define the purpose of the ship. Combat ships normally have weapons as their "missions" while colony ships have colony pods as primary mission.

As of version 0.4, space combat in Zvjezdojedac is much like the combat in OGame, a few rounds of roll based shooting. In Zvjezdojedac players have a little bit more control over the space combat, ships can be moved in 1D space closer to or further away from the enemy.

# Research #

Research is also big part of 4X strategies especially those where player starts small and has whole galaxy (or planet) to conquer. In Zvjezdojedac there are two means of scientific improvement, research and development. Research is about theories while development is about applications. Development is directly influenced by resources devoted to it and produces concrete effects such as unlocking new ship components, buildings and other kind of improvements. Research on the other hand is influenced only by the best developed star system and only unlocks new development topics. That way both small and large economies have same "depth" of the research but large one broader "breadth" of development.

# Diplomacy and espionage #

As of version 0.4 both diplomacy and espionage are not implemented nor thoroughly designed. It is planned to have some form of trading and stealing technologies.

# Name of the project #

The name of the project has a story too. In 4X space games number of stars on the "galaxy map" is usually limited to few hundreds. While it would surely be impractical to play on bigger maps players may wonder why is galaxy such a small place. This game uses "mysterious pocket universe" approach to answer that question. On top of that, the "pocket universe" is inside of huge organic beast, in some way inspired by a certain episode of Star Trek: Voyager (5th episode in season 1). The nature of "pocket universe" also opens a lot of possibilities for game mechanics such as an ecosystem of space faring organisms that extract energy from stars and parasites that feed on those organisms.