JJ Code Base
============

Larger code base that used to be for multiple applications. But most of these were moved to the Archive folder, because they were migrated to their own separate repositories now:

- [JJ.Framework](https://github.com/jjvanzon/JJ.Framework)
- [JJ.Synthesizer](https://github.com/jjvanzon/JJ.Synthesizer)
- [JJ.SaveText](https://github.com/jjvanzon/JJ.SaveText)
- [Question & Answer](https://github.com/jjvanzon/JJ.QuestionAndAnswer)

What's still left in here unmigrated:

- JJ.Data.Canonical
- JJ.Business.Canonical
- Misc demos

Modularization
--------------

Circle3 was the previous code base, that started being developed around 2010. It was about a quite specific way of programming, not like mainstream .NET development. It had a first-order grouping by functionality.

The way of working was not portable between what I did at home and what I did at the office. My ideas about software architecture have changed.

JJ code base is newer. It started being developed around 2013. Its architecture is more agnostic about specific data models, specific programming models, specific API's and specific functionality. The first order grouping is now by technical aspects such as apps, services, framework, validation, security, etcetera. The idea is that that software architecture might accommodate different types of programming and different types of functionality in an orderly fashion.

I think a first-order grouping by technical aspects instead of by functional aspects may be of help, because technology can change fast, but functionality may change even faster, so for a main outline of things, technical aspects might be more stable as a subdivision, than functional aspects, which may change more frequently, in my opinion.

Setting Up Dev Environment
--------------------------

### JJ.Framework

- Pre-release versions of *JJ.Framework* components could be used.
- They may require configuring *NuGet* to connect to an additional package source.
- The following info might be useful for that:
- Name: JJs-Pre-Release-Package-Feed
- Source: https://pkgs.dev.azure.com/jjvanzon/1de16010-421a-41a5-90f1-86e9513f2c5b/_packaging/JJs-Pre-Release-Package-Feed/nuget/v3/index.json
