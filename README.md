# Solution

### Declarative Ruleset
I have implemented a configurable declarative mechanism to define and iterate through the rules of loan approval.

The rules are defined in `appsettings.json` and are loaded at application start. The `LoanApprovalService` matches
the current loan application against the collection of rules.



|MaxLoanAmount|MaxLoanToValueRatio|MinCreditScore|
|-------------|-------------------|--------------|
|1000000      |0.6                |750           |
|1000000      |0.8                |800           |
|1000000      |0.9                |900           |
|1500000      |0.6                |950           |


The benefit of this approach is that rule bands can be altered/extended in the future without any change to code.


### Assumptions and TODO
Any changes required or recommended for a production release are marked in the code with `// TODO`.

Any assumptions are marked `// ASS`. 😊

