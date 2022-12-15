# Fuzzy Logic Activity

The application is split into two modules:

1. InferenceLibrary - a module containing all classes required for performing the inference.

- Performs deffuziffication using Mean of Maxima method
- Allows defining fuzzy rules and optionally: negate conditions, define join operator (or/and)
- Allows displaying all inference steps thanks to the `InferenceDetails` class

An example inference process can be found in the integration tests directory.

2. Windows forms application - application that uses the InferenceLibrary module for performing and displaying the inference details

* Allows setting membership functions ranges and draws their charts
* Displays all defined rules in a human readable format
* Shows all inference process - fuzzification, rules evaluation and aggregated membership chart together with a defuzzification result

3. Fuzzy Logic Rules to be followed

* If (Water is Cold) THEN Power IS Low
* If (Water is Hot) THEN POWER IS High