# Versions #



## Version numbering ##

Version numbering scheme in this project is simple: x.y where x denotates major release (1 for full game, 2 for sequel) and y minor release. Zero for major release (as in v0.2) means that there is no major release yet.

Development builds may have extended schema with up to four numbers: x.y.z.r. X and y are the same as described above. Z is a build number within minor release and is reset after each minor release. R is code base version (repository revision number) at the time of build.


## Version 0.1 ##

Version 0.1 includes all work prior introducing project to Google Code and Sourceforge. It was started somewhere in June 2009 and "completed" on April 24th 2010. Functionality it had:
  * New game can be started
  * Next turn
  * New turn report (only reporting new breakthroughs)
  * Technology research
  * Ship design
  * Ship and building construction

## Version 0.2 ##

Major features of version 0.2 are ability to colonize planets and saving and loading game. Version was completed on August 7th 2010. List of features implemented in v0.2:
  * Colonization
  * GUI for viewing fleet info
  * Mechanism for design upgrade (not ship upgrade)
  * New turn reports improved
  * Planet and star system visibility
  * Planet pictures dependant on planet conditions
  * Saving and Loading

## Version 0.3 ##

Major features of this version are localization mechanism, interstellar travel and in-game technology library. Version was completed on July 9th 2011. List of features in v0.3:
  * Interstellar travel
  * Localization mechanism
    * Croatian translation
    * English translation
  * In-game technology library
  * GUI scaling
  * Population migration
  * System wide effects (such as migration limit)
  * Game can be started with multiple colonized planets

## Version 0.4 ##

Major features of this version are introduction of AI and star system management. Star system is a layer between galaxy and colonies. Starship construction and technology development became part of star system activities. AI implementation was rudimentary one, it's implementation was foundation for future work. Version was completed on August 10th 2012. List of features in v0.4:
  * Star system management
  * Space combat
  * List of star systems and colonies
  * Rudimentary AI

## Version 0.5 (current) ##

This is current phase of development. For planned features check [issues for milestone 0.5](http://code.google.com/p/zvjezdojedac/issues/list?can=1&q=Milestone%3D0.5+&colspec=ID+Type+Status+Priority+Milestone+Owner+Summary&cells=tiles)