## Програмний функціонал
В грі доступно 3 рівня - легкий (4 на 4), нормальний (9 на 9) та складний (16 на 16). Гравцю доступні підсказки:

+ Рандомне відкриття клітинка

+ Відкриття конкретної клітинки

та відміна ходу, також є рейтинг за кожним рівнем.

## Programming Principles

**KISS**

Більшість методів, які є в програмі мають просту логіку, що дає легко зрозуміти, що кожен метод виконує, приклади:
	
* [GenerateBoard](./MySudokuGame/SudokuComponents/SudokuGame.cs#L28-L37)
	
* [Check](./MySudokuGame/SudokuComponents/SudokuGame.cs#L56-L66)
	
* [InitializeBoard](./MySudokuGame/MySudoku/Game.xaml.cs#L72-L86)

**DRY**

Частини коду програми, що повторюються винесені в окремі методи, прикладии:
	
* [StartStopWatch](./MySudokuGame/MySudoku/Game.xaml.cs#L50-L56)
	
* [Redirection](./MySudokuGame/MySudoku/Game.xaml.cs#L160-L165)

**YAGNI**

В програмі реалізовани основний функціонал, який відповідає MVP

**Open/Closed**

Реалізовано його в бібліотеці класів [Hints](./MySudokuGame/Hints). Тобто у нас є загальний інтерфейс IHits, який має 
свої методи, а класи OpenRandomCell та OpenSpecificCell  можуть їх перевизначати та додавати свої унікальні методи.

**Single Responsibility Principle**

Класи [SudokuGame](./MySudokuGame/SudokuComponents/SudokuGame.cs), [Board](./MySudokuGame/SudokuComponents/Board.cs),
 [Generator](./MySudokuGame/Solver/Generator.cs) вирішують лише одну здачу.

## Design Patterns

+ Memento

Реалізований паттер для класу [Board](./MySudokuGame/SudokuComponents/Board.cs), основна реалізація в 
папці [Memento](./MySudokuGame/SudokuComponents/Memento). Використовується для відміни ходу гравця.

+ Facade

Використовується даний паттерн в [SudokuGame](./MySudokuGame/SudokuComponents/SudokuGame.cs) та 
[Game.xaml.cs](./MySudokuGame/MySudoku/Game.xaml.cs). Використовується для того, щоб мінімізувати зв'язок між 
класами.

+ Command

Реалізований у бібліотеці класів [Hints](./MySudokuGame/Hints). Використовується, щоб викликати різні підсказки в залежності 
від вибору користувача.

+ Singleton

Реалізований в папці [Rating](./MySudokuGame/Rating) клас [RatingGenerator](./MySudokuGame/Rating/RatingGenerator.cs). Використовується для того, щоб забезпечити спільний доступ, до файлу.

## Refactoring Techniques

+ Replace Data Value with Object

Використовується в [Board](./MySudokuGame/SudokuComponents/Board.cs#L15)

+ Rename Method

Методи маю зрозумілі назви, що дає зрозуміти програмісту, що даний метод робить.

+ Extract Class

Використовується в [SudokuGame](./MySudokuGame/SudokuComponents/SudokuGame.cs)
