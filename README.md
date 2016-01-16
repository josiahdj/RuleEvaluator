# RuleEvaluator
A hierarchical/recursive eligibility rule evaluation engine with administrative override feature.

The business end of the library is in the `EligibilityEvaluator` class, and the actual evaluation logic is in the `EligibilityRuleEvaluator` class.

This library was created to traverse a tree-structured set of atomic rules which are stored in a database and spit out a true/false for the whole set.

The rules are represented this way:
```
RuleContainer (e.g. Customer, User, Company, etc.)
    \----RuleSet
        \----Rule
        \----Rule
    \----RuleSet
        \----RuleSet
            \----Rule
        \----Rule    
```

There is also a `RuleContext` (I genericized it for publication, but this could be a shopping cart or a user or anything that you want to determine eligibility about).

I've included some sample rules in the Rules folder.
