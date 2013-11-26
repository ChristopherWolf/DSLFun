# DSL Research

## TODO: Arrange this into readable sections

## External DSL Topics

### Terminal Symbols

Terminal symbols are literal symbols which may appear in the inputs to or outputs from the production rules of a formal grammar and which cannot be changed using the rules of the grammar (this is the reason for the name "terminal").
For concreteness, consider a grammar defined by two rules:

1. x can become xa
2. x can become a

Here a is a terminal symbol because no rule exists which would change it in to something else. (On the other hand, x has two rules that can change it, thus it is nonterminal.) A formal language defined (or generated) by a particular grammar is the set of strings that can be produced by the grammar and that consist only of terminal symbols.

### Nonterminal Symbols

Nonterminal symbols are those symbols which can be replaced. They may also be called simply syntactic variables. A formal grammar includes a start symbol, a designated member of the set of nonterminals from which all the strings in the language may be derived by successive applications of the production rules. In fact, the language defined by a grammar is precisely the set of terminal strings that can be so derived.

#### Example

For instance, the following represents an integer (which may be signed) expressed in a variant of Backus–Naur form:

<integer> ::= ['-'] <digit> {<digit>}
<digit> ::= '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9'

In this example, the symbols (-,0,1,2,3,4,5,6,7,8,9) are terminal symbols and <digit> and <integer> are nonterminal symbols.

### Backus–Naur Form

### Syntax Trees

#### Parse Tree

#### Abstract Syntax Tree

## Internal DSL Topics