How to Configure your searchs ?
Right now, i don't have an UI for you, but you can edit "Searchs.json"

Make sure to respect the format !!

searchName : "The name of your search. This is what display the button in the Search panel"
searchType : "system", "body"  --> This is the type of search (more to come)

searchDisplay : nameNotes,allegiance,government,state,economy,secondEconomy,reserve,security,population
==> the properties to display in the search panels. It's to limited 6, but keep in mind some may be cropped or not appear if you add too much

searchParams : 
    [ "ParamaterName" : "ParameterValue "],
    [ "ParamaterName" : "ParameterValue "],
    [ "ParamaterName" : "ParameterValue "],
    ...

    ---> this is the paramaters for the search. You need at least one parameter.

searchMaxRadius : You can specify a maximum radius to speedup the search. The maximum is 100. 
                  The default value is 100 for system and 20 for bodies.
                  Note that increasing the distance increase the search time.

========================
SYSTEM SEARCH PARAMETERS
========================
you can use ';' to enter several values
note that the operator AND will be used between parameter and OR between value of a parameter

Search operate in a radius (max:100) around your current location.
It will return the 10 closest locations matching the parameters

--------------------------------------------------------------------------------------

name: the exact name of a system. THIS PARAMETER WILL CAUSE ALL OTHER (except nameNotes) TO BE IGNORED. With the ';' you can build a list of systems. See the "double painite" example

nameNotes: SPECIAL. Use only with "name" parameter. This will display a note in the search result for each entry. See the "double painite" example

allegiance: alliance, empire, federation, independent, guardian, pilots federation, none

government : anarchy, colony, communism, confederacy, cooperative, corporate, democracy, dictatorship, feudal, imperial, none, patronage, prison colony, theocracy, engineer

state : blight, boom, bust, civil liberty, civil unrest, civil war, cold war, colonisation, damaged, drought, election, expansion, famine, historic event,
        infrastructure failure, investment, lockdown, natural disaster, none, outbreak, pirate attack, public holiday, retreat, revolution, technological leap,
        terrorist attack, trade war, under repairs, war

economy :   agriculture, extraction, high tech, industrial, military, prison, refinery, service, terraforming, tourism, colony, damaged, repair, rescue, none,

reserve :  depleted, low, common, major, pristine

security : low, medium, high, anarchy, lawless

========================
BODY SEARCH PARAMETERS
========================
additionnal search display for bodies: 
searchDisplay: materials, distanceToArrival

isLandable: true, false

rawMaterial: Any raw materials. If you search for several materials at a time, it will return a match with ALL of this materials. It can increase the search time. I recommand to search for a material at a time. You can find the list of raw materials here: https://inara.cz/galaxy-components/
