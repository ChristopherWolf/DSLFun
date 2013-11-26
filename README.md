# DSL Research

## TODO: Arrange this into readable sections

## External DSL Topics

### Definitions
- Semantic Model: The model populated by the DSL

- Recursive Descent Parser
A recursive descent parser is a kind of top-down parser built from a set of mutually recursive procedures (or a non-recursive equivalent) where each such procedure usually implements one of the production rules of the grammar. Thus the structure of the resulting program closely mirrors that of the grammar it recognizes.

- Parser Combinator

- Parser Generator (Eg: ANTLR)

ANTLR takes as input a grammar that specifies a language and generates as output source code for a recognizer for that language
A language is specified using a context-free grammar which is expressed using Extended Backus–Naur Form (EBNF).


### Steps to get from source DSL to executable code
1. Lexical Analysis (Lexing)

Lexical analysis is the process of converting a sequence of characters into a sequence of tokens.

Tokens are easier for the parser to understand than raw characters.

Generally lexical grammars are context-free or almost context-free, and do not require any looking back, looking ahead, or backtracking, which allows a simple, clean, and efficient implementation. This also allows simple one-way communication from the lexer to the parser, without needing any information flowing back to the lexer.

The first stage is the token generation, or lexical analysis, by which the input character stream is split into meaningful symbols defined by a grammar of regular expressions. For example, a calculator program would look at an input such as "12*(3+4)^2" and split it into the tokens 12, *, (, 3, +, 4, ), ^, 2, each of which is a meaningful symbol in the context of an arithmetic expression. The lexer would contain rules to tell it that the characters *, +, ^, ( and ) mark the start of a new token, so meaningless tokens like "12*" or "(3" will not be generated.

2. Parsing (Syntactical Analysis)

Takes stream of tokens from lexer and arranges into parse tree and AST.

The next stage is parsing or syntactic analysis, which is checking that the tokens form an allowable expression. This is usually done with reference to a context-free grammar which recursively defines components that can make up an expression and the order in which they must appear. However, not all rules defining programming languages can be expressed by context-free grammars alone, for example type validity and proper declaration of identifiers. These rules can be formally expressed with attribute grammars.

3. Semantic Parsing (Compiler, interpreter or translator)

AST can be walked to populate Semantic Model or to generate source code in a different language.

The final phase is semantic parsing or analysis, which is working out the implications of the expression just validated and taking the appropriate action. In the case of a calculator or interpreter, the action is to evaluate the expression or program, a compiler, on the other hand, would generate some kind of code. Attribute grammars can also be used to define these actions.


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


### Grammars and Backus–Naur Form

#### Regular Grammar

- A regular grammar is a formal grammar that describes a regular language.


#### Context-free grammar
- In formal language theory, a context-free grammar (CFG) is a formal grammar in which every production rule is of the form

V -> w

where V is a single nonterminal symbol, and w is a string of terminals and/or nonterminals (w can be empty). A formal grammar is considered "context free" when its production rules can be applied regardless of the context of a nonterminal. It does not matter which symbols the nonterminal is surrounded by, the single nonterminal on the left hand side can always be replaced by the right hand side.


### Syntax Trees

#### Parse Tree
- An exact representatoin of your code's syntax according to your grammar

#### Abstract Syntax Tree
- A tree representation of the abstract syntactic structure of source code. The syntax is "abstract" in not representing every detail appearing in the real syntax. For instance, grouping parentheses are implicit in the tree structure, and a syntactic construct like an if-condition-then expression may be denoted by means of a single node with two branches.

## Internal DSL Topics