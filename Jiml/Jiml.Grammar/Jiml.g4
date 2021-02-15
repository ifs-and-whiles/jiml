grammar Jiml;

json
 : value EOF
 ;

object
 : '{' conditionalProperty (',' conditionalProperty)* '}'	#ObjectRule
 | '{' '}'													#EmptyObjectRule
 ;

conditionalProperty
 : property													#PropertyRule	
 | '?' condition '->' property ('|' conditionalProperty)?	#IfElsePropertyRule
 ;

property
 : STRING ':' value
 ;

array
 : '[' conditionalElement (',' conditionalElement)* ']'		#ArrayRule
 | '[' ']'													#EmptyArrayRule
 ;

conditionalElement 
 : element													#ElementRule		
 | '?' condition '->' element ('|' conditionalElement)?		#IfElseElementRule
 ;

element
 : value				#ValueElementRule
 | '...' value			#SpreadElementRule
 ;
 
condition
 : '(' condition ')'						#SubConditionRule
 | '!' condition							#NegationRule
 | left=condition '&&' right=condition		#AndRule
 | left=condition '||' right=condition		#OrRule
 | left=expression '==' right=expression	#EqualityRule
 | left=expression '!=' right=expression	#NotEqualityRule
 | left=number '>' right=number				#GreaterThanRule
 | left=number '>=' right=number			#GreaterOrEqualToRule
 | left=number '<' right=number				#LessThanRule
 | left=number '<=' right=number			#LessOrEqualToRule
 | boolean									#BoolRule
 ;

value
 : expression
 | condition
 ;

expression
 : '(' expression ')'																									#ParensValueRule
 | '?' condition '->' ifVal=expression '|' elseVal=expression															#IfElseValueRule
 | source=expression '>>' '(' x=VAR (',' i=VAR)? ')' '->' '(' dest=expression ')'										#MapRule
 | source=expression '?>' '(' x=VAR (',' i=VAR)? ')' '->' '(' dest=condition	')'										#FilterRule
 | source=expression '><' accVal=expression ',' '(' acc=VAR ',' x=VAR (',' i=VAR)? ')' '->' '(' dest=expression ')' 	#ReduceRule
 | variable																												#VariableValueRule
 | STRING																												#StringValueRule
 | number																												#NumberValueRule
 | object																												#ObjectValueRule
 | array																												#ArrayValueRule
 | boolean																												#BooleanValueRule
 | NULL																													#NullValueRule
 ;	
 
number
 : INTEGER												#IntegerRule
 | INTEGEREXP											#IntegerExpRule
 | DECIMAL												#DecimalRule
 | variable												#VarNumberRule
 | '(' number ')'										#ParensNumberRule
 | x=number '^' y=number								#PowerRule
 | '-' number											#NegativeNumberRule
 | x=number (TIMES | BY) y=number						#MultiplicationRule
 | x=number (PLUS | MINUS) y=number						#AdditionRule
 ;

variable
 : VAR						#VariableRule
 | variable '.' VAR			#VariableFieldRule
 | variable rangeIndex		#VariableRangeIndexRule
 | variable pickIndex		#VariablePickIndexRule
 ;
 
rangeIndex
 : '[' (from=number)? ':' (to=number)? ']'
 ;

pickIndex
 : '[' conditionalPickIndexElement (',' conditionalPickIndexElement)* ']'
 ;

conditionalPickIndexElement
 : number															#PickIndexElementRule
 | '?' condition '->' number ('|' conditionalPickIndexElement)?		#IfElsePickIndexElementRule
 ;
 
boolean
 : TRUE		#TrueRule
 | FALSE	#FalseRule
 ;
  
PLUS
 : '+'
 ;

MINUS 
 : '-'
 ;

TIMES
 : '*'
 ;

BY
 : '/'
 ;

STRING   
 : '"' (ESC | SAFECODEPOINT)* '"'
 ;

fragment ESC
 : '\\' (["\\/bfnrt] | UNICODE)
 ;

fragment UNICODE
 : 'u' HEX HEX HEX HEX
 ;

fragment HEX
 : [0-9a-fA-F]
 ;

fragment SAFECODEPOINT
 : ~ ["\\\u0000-\u001F]
 ;

INTEGER			
 : INT
 ;

INTEGEREXP
 : INT EXP
 ;

DECIMAL
 : INT '.' [0-9]+ EXP?
 ;

fragment INT
 : '0' 
 | [1-9][0-9]*
 ;
   
fragment EXP
 : [Ee] [+\-]? INT
 ;

NULL
 : [n][u][l][l]
 ;

TRUE
 : [t][r][u][e]
 ;

FALSE
 : [f][a][l][s][e]
 ;

VAR	
 : [a-zA-Z_][a-zA-Z0-9_]* 
 ;

WS				
 : [ \r\t\u000C\n]+ -> skip 
 ;