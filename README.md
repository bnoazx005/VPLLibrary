**Название**:    VPLLibrary

**Автор**:       Касимов Ильдар

**e-mail**:      ildar2571@yandex.ru 

**Версия**:      1.0

**Описание**:

VPLLibrary --- это библиотека предназначенная для работы с абстрактными синтаксическими деревьями,
описывающими язык VPL. В поставку библиотеки также входит интерпретатор данного языка.
       
Проект был разработан с использованием среды Microsoft Visual Studio 2017 Community.

**БНФ грамматика языка VPL**

Язык VPL описывается следующей грамматикой в форме Бэкуса-Наура:

    <program> ::= <operation-list>
      
    <operation-list> ::=   <identifier> <- <expression>
                         | <identifier> <- <expression> <operation-list>

    <expression> ::= <identifier>
     | <value>
     | READ (<array-index>)
     | NULL
     | HEAD (<expression>)
     | LAST (<expression>)
     | SUM (<expression>)
     | REVERSE (<expression>)
     | MAX (<expression>)
     | MIN (<expression>)
     | SORT (<expression>)
     | CONCAT (<expression>, <expression>)
     | SLICE (<expression>, <array-index>, <array-index>)
     | INDEXOF (<expression>, <integer-value>)
     | GET (<expression>, <array-index>)
     | LEN (<expression>)
     | MAP (<lambda-function>, <expression>)
     | FILTER (<lambda-predicate>, <expression>)
     | IF <identifier> <lambda-predicate> THEN <expression> ELSE <expression>
     | VECOP (<lambda-function-2>, <expression>, <expression>)

    <array-index> ::= <identifier> | <integer-value>

    <lambda-function> ::= (<op> <lambda-function-body>)

    <op> ::= + | - | * | / | % | **

    <lambda-function-body> ::= <integer-value>
     | <lambda-function>

    <lambda-function-2> ::= (<op>) | (MAX) | (MIN)

    <lambda-predicate> ::= (<lop> <integer-value>)
     | (% <integer-value> == 1) | (% <integer-value> == 0)

    <lop> ::= > | < | >= | <=| == | !=

    <value> ::= <array-value> | <integer-value>

    <array-value> ::= [ <array-members> ]

    <array-members> ::= <integer-value>
     | <integer-value> , <array-members>

    <integer-value> ::= <digit> 
     | - <integer-value>
     | <integer-value> <digit>

    <identifier> ::= <char> 
     | <identifier> <digit>
     | <identifier> <char>

    <char> ::= a | b | c |\( \dots \)| z | A | B | C |\( \dots \)| Z

    <digit> ::= 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9

Каждый оператор языка записывается в отдельной строке.

**Особенности языка**

Приведённый выше язык является весьма ограниченным, в нём отсутствует достаточно большая часть функционала, который может быть реализован над массивами. Но с ростом возможностей также начинает возрастать сложность поиска нужного решения в пространстве поиска. Язык основывается на функциональном подходе, используемом в LISP за исключением применения рекурсии и ряда других возможностей, а также на основе предметно-ориентированного языка, используемого в технологии DeepCoder. Рассматриваемый язык обладает набором встроенных функций первого (HEAD, LAST, SUM, REVERSE, MAX, MIN, SORT, CONCAT, SLICE, INDEXOF, GET) и второго порядков (MAP, FILTER, VECOP). В языке присутствует конструкция, позволяющая задавать условное выполнение операторов. Также имеется поддержка простейших лямбда-выражений и предикатов. Лямбда-функции записываются с применением префиксной записи, т.е. функция вида

$$(+(*2))$$

эквивалента следующей 

$$f(x) = x + 2 * x$$,

имя переменной при этом не указывается, но подразумевается. Значение последнего оператора считается результатом работы программы.

В языке присутствует возможность работы как с одиночными целыми числами, так и с массивами. Однако на уровне реализации интерпретатора первые преобразуются к массивам, состоящим из одного элемента. Также существует нулевой элемент, обозначаемый идентификатором NULL. Также приняты дополнительные ограничения с целью упрощения интерпретации: индексы массивов могут принимать любое целочисленное значение, конечный индекс расчитывается по формуле 

$$i_{wrapped} = (length + i~\%~length)~\%~length$$;

все встроенные функции могут принимать в качестве аргументов значения-массивы, даже если в действительности требуется передача числового значения; операции деления и взятия остатка от деления могут быть дополнены, чтобы избегать ситуации деления на нуль. При этом работа с переменными требует их предварительного объявления. Последнее осуществляется присвоением некоторому имени значения, например:

    x <- NULL
    y <- 42
    z <- VECOP((+), x, y)

использование необъявленной переменной приведёт к ошибке времени исполнения.

**Замечание:** для включения проверки деления на нуль требуется указать флаг E_INTERPRETER_ATTRIBUTES.IA_IS_SAFE_DIVISION_ENABLED при создании экземляра интерпретатора. В случае возникновения данной ситуации результатом операции будет первый операнд (делимое).