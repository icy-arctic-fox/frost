Tokens
------

### Numerical ###
These tokens represent some static numerical value.

* `[0-9]+` Integer
* `[0-9]+.[0-9]+` Float
* `0x[0-9A-Fa-f]+` Hex
* `0b[01]+` Binary

### Built-in Types ###

* `int` Integer
* `float` Float
* `string` String

### Strings ###

* `[_A-Za-z][_A-Za-z0-9]*` Identifier
* `'((\\')|[^'])+'` QuotedString
* `"((\\")|[^"])+"` DoubleQuotedString

### Keywords ###

* `def` Definition
* `function` Function
* `class` Class
* `new` New Instance

### Punctuation ###

* `(` LeftParen
* `)` RightParen
* `[` LeftBrace
* `]` RightBrace
* `{` LeftCurlyBrace
* `}` RightCurlyBrace
* `.` Dot
* `;` Semicolon
* `:` Colon
* `,` Comma

**Notes:**

* Whitespace is ignored unless it appears inside quotes.
* Comments are "C-styled"
	`//` denotes a single line comment, everything that follows `//` is ignored.
	`/* ... */` denotes a multi-line comment, everything between `/*` and `*/` is ignored.
	Comments inside of quotes (strings) are treated literally and are not ignored.


Syntax
------

At the top-level, all files contain a *definition list*. That list can contain any number (zero or more) *definitions*.

### Definition List ###
Zero or more of *Definition*.

**Syntax:**
`Definition*`

### Definition ###
Defines a function, class, or other pluggable type.
Each definition has a name and content.

#### Function Definition ####
Defines a function block that can be referenced and executed by name.
Functions can have an argument list.
If the function doesn't accept any arguments, then the argument list should be empty, leaving just a pair of parenthesis.
Any number of statements can be listed inside of the function.

**Syntax:**
`Definition` `Function` `Identifier(name)` `LeftParen` *`ArgumentList(args)`* `RightParen` `LeftCurlyBrace` *`StatementList(statements)`* `RightCurlyBrace`

**Examples:**

	def function do_something () {
		// ...
	}

	def function say_hello (name) {
		// ...
	}  

#### Class Definition ####
Defines a class.
A class is a collection of function definitions and values.

**Syntax:**
`Definition` `Class` `Identifier` `LeftCurlyBrace` *`ClassList`* `RightCurlyBrace`

**Examples:**

	def class EmptyClass { }
	
	def class MathHelp {
		def function factorial (num) {
			// ...
		}
	}
	
	def class DataClass {
		var a;
		var b;
		def function do_work ()	{
			// ...
		}
	}

#### Pluggable Type Definition ####
Pluggable types allow for customized definitions.
Pluggable types are defined in the engine or extensions of the engine.

**Syntax:**
`Definition` `Identifier(type)` `Identifier(name)` `LeftParen` *`ArgumentList(args)`* `RightParen` `LeftCurlyBrace` *`StatementList(statements)`* `RightCurlyBrace`

**Examples:**

	def console_command load_map (name) {
		// ...
	}
	
	def chat_command spawn_enemy (enemy) {
		// ...
	}