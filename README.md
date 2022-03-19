# SearchEngineRanker
SearchEngineRanker

--------------------
Searcher Version 1.0
--------------------

- The application starting point is the WPF project.
- please run from Visual Studio


-----------------------
Structure of the code:
----------------------

- kept it simple
- search is performed against Google (other search engines could be implemented)
- the result is returned and sent to a results processor/parsed.

- the results are parsed using Regex.
- the search result parsed are then passed to a RankingFinder
- then the rankings returned are then sent to a RankingFormatter.

- The RankingFormatter could also take a finder as parameter, and return formatted events.

---------------------------------------------------------------------
If I  had more time and/or needed more commands (Or for a version 2)
---------------------------------------------------------------------
- I would use a MVVM pattern or a framework such as Prism (https://prismlibrary.com/docs/)


-------------
PLEASE NOTE:
-------------

- I'm using a very nice FontAwesome library for PDF. Unfortunately .NET 6.0 is not supported fully yet. However for the purposes of this exercise I feel this is ok.


-----------------------------------
Thanks for reading the Readme.txt
-----------------------------------
