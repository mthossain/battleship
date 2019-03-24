using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FlareHRBattleship
{
    // Ship class for battleships
    public class Ship {
        private char identifier;
        private int hit = 0;

        public Ship(char identifier){
            this.identifier = identifier;
        }

        // Registers hit
        public void hitTaken(){
            hit++;
        }

        public bool isSunk(){
            // If the ship has taken 3 hits, then it's sunk
            if (hit>=3)
                return true;
            else
                return false;
        }

        public char getIden(){
            return identifier;
        }

    }
    
    public class Program
    {
        // Method to print the game board
        static void printBoard(char[,] gameBoard){
            // Printing 0-9 grid numbers for columns
            Console.Write("  ");
            for (int i=0; i<gameBoard.GetLength(0); i++){
                Console.Write(i+" ");
            }
            Console.WriteLine();
            // Printing the entire board
            for (int i=0; i<gameBoard.GetLength(0); i++){
                Console.Write(i+" ");
                for (int j=0; j<gameBoard.GetLength(1); j++){
                    Console.Write(gameBoard[i,j]+" ");
                }
                Console.WriteLine();
            }
        }

        // Method to take row/column inputs from the user
        static int input(String message){
            int output = 0;
            bool exit = false;
        
            do {
                Console.Write(message);
                var readInput = Console.ReadLine();
                if (int.TryParse(readInput, out output)){
                    if (output >=0 && output <10){
                        exit = true;
                    }
                    else{
                        Console.WriteLine("Invalid input. Please enter an input in the valid range.");
                    } 
                }
                else{
                    // The system will exit if user gives 'exit' input at any time 
                    if (readInput.ToLower() == "exit") {
                        Environment.Exit(0);
                    }
                    else{
                        Console.WriteLine("Invalid input. Please enter a valid input.");
                    }   
                }
            } while (!exit);
            
          
            return output;
        }

        // Method to take ship's orientation input from the user
        static char orientationInput(){
            char output = default(char);
            bool exit = false;
            Console.WriteLine("To put your ship horizontally, enter 'h' or enter 'v' to put it vertically");
            do {
                Console.Write("Input: ");
                var readInput = Console.ReadLine();
                if ( char.TryParse(readInput, out output)){
                    output = char.ToLower(output);
                    // Determines if the input is horizontal or vertical
                    if (output == 'v' || output == 'h'){
                        exit = true;
                    }
                    else{
                        Console.WriteLine("Invalid input. Please enter a valid input.");
                    }       
                }
                else{
                    // The system will exit if user gives 'exit' input at any time 
                    if (readInput.ToLower() == "exit") {
                        Environment.Exit(0);
                    }
                    else{
                        Console.WriteLine("Invalid input. Please enter a valid input.");
                    }   
                }
            } while (!exit);
            
          
            return output;
        }

        // Method to check if the ship's position is inside the board
        static bool isValidCoord(int row, int col, char orientation){
            // Game Board size is 10, so the max index becomes 9
            int boardMaxIndex = 9;
            if (orientation == 'h'){
                // By default the ship's size is 3, so it checks if the ship has starting point + 2 points inside the game board
                if ((boardMaxIndex-col)<2){
                    Console.WriteLine("Invalid location. Ship is outside of the board.");
                    return false;
                }    
            }
            else{
                // By default the ship's size is 3, so it checks if the ship has starting point + 2 points inside the game board
                if ((boardMaxIndex-row)<2){
                    Console.WriteLine("Invalid location. Ship is outside of the board.");
                    return false;
                }    
            }
            return true;
        }

        static void Main(string[] args)
        {
            // Starting messages
            Console.WriteLine("Welcome to the game of Battleship");
            Console.WriteLine("This is a task for Flare HR");
            Console.WriteLine("By default, you can add only 1 battleship");
            Console.WriteLine("By default, the battleship's size is 3");
            Console.WriteLine("At anytime, if you write 'exit' in the input, the game will shut down instantly");
            Console.WriteLine("Let's play the game...........");

            // Game board size
            int size = 10;
            // Game board 2d array
            char[,] gameBoard = new char[size,size];
            // For this test, we are making only 1 ship object but we can make as many as we want
            Ship ship_1 = new Ship('S');
            // Dictionary to hold all battleships. Using dictionary for extensibility. So it can be extended to add more ships. As well as, dictionary is faster, so the ship lookup time is Constant ( O(1) ).
            Dictionary<char, Ship> ships = new Dictionary<char, Ship>();
            // Adding one battleship (One just for this task's purpose, gives option to add more) to the battleship holder dictionary.
            ships[ship_1.getIden()] = ship_1;
            // Ship's starting row, column and orientation variables
            int row;
            int col;
            char orientation;

            // Basic game board setup
            for (int i=0; i<gameBoard.GetLength(0); i++){
                for (int j=0; j<gameBoard.GetLength(1); j++){
                    gameBoard[i,j] = '.';
                }
            }
            // Printing the game board
            printBoard(gameBoard);

            // Taking input for the ship's position
            do {
                Console.WriteLine("Please enter ship's starting location. Ship size is by default 3");
                row = input("Enter row: ");
                col = input("Enter column: ");
                orientation = orientationInput();
            } while(!isValidCoord(row, col, orientation));

            // Adding coords to the map
            gameBoard[row,col] = 'S';
            if (orientation == 'h'){
                gameBoard[row,col+1] = 'S';
                gameBoard[row,col+2] = 'S';
            }
            else{
                gameBoard[row+1,col] = 'S';
                gameBoard[row+2,col] = 'S';
            }

            // Printing the game board
            printBoard(gameBoard);

            // Attacking row and column variables
            int attackRow;
            int attackCol;

            // Taking input for the ship's attacking location
            do {
                Console.WriteLine(ships.Count+" Battleship(s) remaing");
                Console.WriteLine("Put a location to attack");
                attackRow = input("Enter Row: ");
                attackCol = input("Enter Column: ");

                // Checking attack
                // Checks if the current location contains any ships' identification character
                if (ships.ContainsKey(gameBoard[attackRow, attackCol])){
                    char locatedShip = gameBoard[attackRow, attackCol];
                    // When it's a hit, mark that location with an 'X'
                    gameBoard[attackRow, attackCol] = 'X';
                    Console.Beep();
                    Console.WriteLine("------> HIT <------");
                    // Ship's hit taken is registered
                    ships[locatedShip].hitTaken();
                    // If the ship is sunk, gets removed from the ship dictionary (ship holder)
                    if(ships[locatedShip].isSunk()){
                        ships.Remove(locatedShip);
                    }
                }
                else{
                    // When it's a miss shot
                    if(gameBoard[attackRow, attackCol] != 'X'){
                        gameBoard[attackRow, attackCol] = 'M';
                        Console.WriteLine("------> MISS <------");
                    }
                    
                }
                // Prints the game board
                printBoard(gameBoard);
            
            } while (ships.Count!=0);   // Loops till there is no ship remaining in the ship dictionary (ship holder)

            // Prints game over message. In this case, it prints out this message anyway when the game is over but it gives extensibility to add more features in future
            if (ships.Count == 0){
                Console.WriteLine("Game over. All battleships are sunk");
            }
        }
    }
}
