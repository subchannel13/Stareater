# Use of formulas #

Formulas are used in almost all data files in Zvjezdojedac. They describe how to calculate values of attributes and allow some of application logic to be exposed in easily modifiable data files instead of being hardcoded.

For example line `CIJENA = * 1000000 ^ LVL 2` in technology data file means that value for attribute `CIJENA`  is calculated using formula described with `* 1000000 ^ LVL 2`. This example demonstrates use of all three kinds of elements in formulas: operators, constants and variables (`LVL`). During run-time calculations, all values (constants, variables, intermediate results, final result) are System.double.

## Prefix and postfix notation ##

For easier parsing, formulas can be either in prefix or postfix notation. More about these notations can be found on other web sites (ie. [prefix](http://en.wikipedia.org/wiki/Polish_notation) and [postfix](http://en.wikipedia.org/wiki/Reverse_Polish_notation) pages on Wikipedia). This page presumes reader is familiar with prefix or postfix notation. Table below shows example expressions written in all three notation.

| Infix | Prefix | Postfix |
|:------|:-------|:--------|
| `2+3` | `+ 2 3` | `2 3 +` |
| `(a+b)^2` | `^ + a b 2` | `a b + 2 ^` |

# Constants #

Constant can be anything that `double.TryParse` accepts with `NumberStyles.Float` as number style parameter (with period as decimal separator). Following table shows examples of valid and invalid constant expressions.

| Value | Valid | Invalid |
|:------|:------|:--------|
| 12 thousands | `12000` | `12,000` |
| one and half | `1.5` | `1,5`   |
| 25 millions | `25e6` | `25E6`  |
| one-thousandth | `1e-3` |         |

# Variables #

Anything that is neither operator nor constant is considered to be a variable.

# Operators #

## Unary operators ##

| Code | Description | Example | Result |
|:-----|:------------|:--------|:-------|
| `ABS` | Returns absolute value of number. | `ABS -15.75` | 15.75  |
| `INT` | Truncates decimal part using. | `INT -15.75` | -15    |
| `FIX` | Same as `INT` |         |        |
| `TRUNC` | Same as `INT` |         |        |
| `ROUND` | Mathematical rounding<sup>*</sup>. | `ROUND 6.61` | 7      |
| `SGN` | Returns result of signum function. | `SGN -15.75` | -1     |
| `SIGN` | Same as `SGN` |         |        |
| `FLOOR` | Rounds number down. | `FLOOR -15.75` | -16    |
| `CEIL` | Rounds number up. | `CEIL -15.75` | -15    |

<sup>*</sup> Notes:
  * `ROUND` rounds number to nearest integer using `System.Math.Round` with `MidpointRounding.AwayFromZero` as `mode` parameter meaning `ROUND 6.5` would return 7 while `ROUND -6.5` would be -7, not -6.

## Binary operators ##

| Code | Description | Example | Result |
|:-----|:------------|:--------|:-------|
| `+`  | Returns sum of operands. | `+ 11 5` | 16     |
| `-`  | Returns difference of operands. | `- 10 3` | 7      |
| `*`  | Returns product of operands. | `* 4 6` | 24     |
| `/`  | Returns quotient of operands. | `/ 10 4` | 2.5    |
| `^`  | Returns first operand raised to the power of second operand. | `^ 5 3` | 125    |
| `POW` | Same as `^`. |         |        |
| `MIN` | Returns smaller of two operands. | `MIN 2 100` | 2      |
| `MAX` | Returns larger of two operands. | `MAX 2 100` | 100    |
| `DIV` | Calculates integer division of two operands<sup>*</sup>. | `DIV 10 4` | 2      |
| `%`  | Returns reminder of integer division<sup>*</sup>. | `% 10 3` | 1      |
| `MOD` | Same as `%`. |         |        |

<sup>*</sup> Notes:
  * `DIV` rounds down real quotient, meaning `DIV -10 4` would return -3, not -2.
  * `%` works for decimal numbers as well as for negative. Reminder operator is defined as _real quotient_ minus _integer quotient_. `MOD -10 3` returns 1, `MOD 1.2 -2` returns 0.4, `MOD -0.2 1` returns 0.8 (division by 1 is on purpose).

## Ternary operators ##

| Code | Description | Example | Result |
|:-----|:------------|:--------|:-------|
| `ITE` | If then else function, if first operant is _true_ returns second operand otherwise return third opernad (first operand is treated as boolean). | `ITE 2 11 5` | 11     |
| `IF` | Same as `ITE`. |         |        |
| `LIMIT` | Limits first operand to interval defined by second and third operand. If first is less then second, returns second operand, if first is greater then third, returns third operand, otherwise returns first.| `LIMIT 10 2 4` | 4      |
| `FROM` | Interpolates first operand between second and third operand. | `FROM 0.5 10 20` | 15     |
| `BATAK` | Same as `FROM`. |         |        |
| `INTER` | Same as `FROM`. |         |        |
| `LFROM` | Combination of `FROM` and `LIMIT` operators. Interpolates first operand between second and third operand and limits result to interval between second and third operand. | `LFROM 1.5 10 20` | 20     |