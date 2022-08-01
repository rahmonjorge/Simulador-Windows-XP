package main

import (
	"fmt"
	"math/rand"
	"strconv"
	"sync"
	"time"
)

var global1 int = 0
var global2 int = 0
var choice string = ""
var mut1 sync.Mutex
var mut2 sync.Mutex

var colorRed string = "\033[31m"
var colorGreen string = "\033[32m"
var colorYellow string = "\033[33m"
var colorReset string = "\033[0m"
var colorBlue = "\033[34m"
var colorPurple = "\033[35m"
var colorCyan = "\033[36m"

// var colorWhite = "\033[37m"

func main() {
	var exit = false

	for !exit {
		PrintColor("\nWelcome to the concurrency test.", colorYellow)
		fmt.Println("(1) Read explanation of program 1")
		fmt.Println("(2) Execute program 1 with no mutexes")
		fmt.Println("(3) Execute program 1 with mutexes")
		fmt.Println("(4) Execute program 2 with no mutexes")
		fmt.Println("(5) Execute program 2 with mutexes")
		fmt.Println("(0) Exit")
		fmt.Print("> ")
		fmt.Scanln(&choice)

		switch choice {
		case "1":
			fmt.Println("\nThere are two global variables, A and B.")
			fmt.Println("Five threads will be accessing these variables.")
			fmt.Println("")
			fmt.Println("Every thread will INCREASE the value of A by 1, a million times.")
			fmt.Println("Every thread will also DECREASE the value of B by 1, a million times.")
			break
		case "2":
			run(false)
			break
		case "3":
			run(true)
			break
		case "4":
			run2(false)
			break
		case "5":
			run2(true)
			break
		case "0":
			exit = true
			break
		default:
			fmt.Println("Invalid option.")
		}
	}
}

func run(mutexesEnabled bool) {
	var threadCount int = 5
	var wg sync.WaitGroup
	var exit = false

	fmt.Print("\nMutexes enabled:")
	var color string
	if mutexesEnabled {
		color = colorGreen
	} else {
		color = colorRed
	}
	PrintColor(strconv.FormatBool(mutexesEnabled), color)

	fmt.Println("\nPress enter to start.")

	for !exit {
		wg.Add(threadCount)
		global1 = 0
		global2 = 5_000_000

		PrintColor("\n- Initial Values -", colorYellow)
		fmt.Println("A: ", global1)
		fmt.Println("B: ", global2)

		fmt.Scanln(&choice)
		if choice == "x" {
			return
		}

		if mutexesEnabled {
			go mutexthread(&wg)
			go mutexthread(&wg)
			go mutexthread(&wg)
			go mutexthread(&wg)
			go mutexthread(&wg)
		} else {
			go thread(&wg)
			go thread(&wg)
			go thread(&wg)
			go thread(&wg)
			go thread(&wg)
		}

		wg.Wait()

		PrintColor("- Final Values - ", colorYellow)
		fmt.Print("A:", global1)
		if global1 < 5_000_000 {
			missing := 5000000 - global1
			result := fmt.Sprintf("(%d increment operations missing)", missing)
			PrintColor(result, colorRed)
		}
		if global1 == 5_000_000 {
			PrintColor("(no operations missing)", colorGreen)
		}
		fmt.Print("B:", global2)
		if global2 > 0 {
			result := fmt.Sprintf("(%d decrement operations missing)", global2)
			PrintColor(result, colorRed)
		}
		if global2 == 0 {
			PrintColor("(no operations missing)", colorGreen)
		}
		fmt.Print("\nPress enter to restart or X to exit.")
		fmt.Scanln(&choice)
		if choice == "x" {
			exit = true
		}
	}
}

func thread(wg *sync.WaitGroup) {

	for i := 0; i < 1_000_000; i++ {
		global1++ // Critical zone
	}

	for i := 0; i < 1_000_000; i++ {
		global2-- // Critical zone
	}

	wg.Done()
}

func mutexthread(wg *sync.WaitGroup) {

	mut1.Lock()
	for i := 0; i < 1_000_000; i++ {
		global1++ // Critical zone
	}
	mut1.Unlock()

	mut2.Lock()
	for i := 0; i < 1_000_000; i++ {
		global2-- // Critical zone
	}
	mut2.Unlock()

	wg.Done()
}

func run2(mut bool) {
	var wg sync.WaitGroup
	wg.Add(5)
	go increment("[Thread 1]", colorBlue, &wg, mut)
	go increment("[Thread 2]", colorGreen, &wg, mut)
	go increment("[Thread 3]", colorPurple, &wg, mut)
	go increment("[Thread 4]", colorRed, &wg, mut)
	go increment("[Thread 5]", colorYellow, &wg, mut)
	wg.Wait()
	fmt.Print("End of execution. Press enter to return.")
	fmt.Scanln()
}

func increment(name string, color string, wg *sync.WaitGroup, mut bool) {
	start := time.Now()
	s1 := rand.NewSource(start.UnixNano())
	r1 := rand.New(s1)
	loop := 0
	for loop < 5 {
		choice := r1.Intn(2) + 1
		if mut {
			fmt.Println(color, name, "Trying to read the global", choice, colorReset)
		}
		switch choice {
		case 1:
			if mut {
				if mut1.TryLock() {
					fmt.Println(color, name, "Reading global1 as: ", global1, colorReset)
				} else {
					fmt.Println(color, name, "Blocked, awaiting access to global1...", colorReset)
					mut1.Lock()
					fmt.Println(color, name, "Reading global1 as: ", global1, colorReset)
				}
				for i := 0; i < 5; i++ {
					global1 += 1
					time.Sleep(600 * time.Millisecond)
				}
				fmt.Println(color, name, "I changed global1 value. Now it is: ", global1, colorReset)
				mut1.Unlock()
			} else {
				fmt.Println(color, name, "Reading global1 as: ", global1, colorReset)
				for i := 0; i < 5; i++ {
					global1 += 1
					time.Sleep(600 * time.Millisecond)
				}
				fmt.Println(color, name, "I changed global1 value. Now it is: ", global1, colorReset)
			}
		case 2:
			if mut {
				if mut2.TryLock() {
					fmt.Println(color, name, "Reading global2 as: ", global2, colorReset)
				} else {
					fmt.Println(color, name, "Blocked, awaiting access to global2...", colorReset)
					mut2.Lock()
					fmt.Println(color, name, "Reading global2 as: ", global2, colorReset)
				}
				for i := 0; i < 5; i++ {
					global2 += 1
					time.Sleep(600 * time.Millisecond)
				}
				fmt.Println(color, name, "I changed global2 value. Now it is: ", global2, colorReset)
				mut2.Unlock()
			} else {
				fmt.Println(color, name, "Reading global2 as: ", global2, colorReset)
				for i := 0; i < 5; i++ {
					global2 += 1
					time.Sleep(600 * time.Millisecond)
				}
				fmt.Println(color, name, "I changed global2 value. Now it is: ", global2, colorReset)
			}
		}
		loop += 1
	}
	end := time.Now()
	fmt.Println(color, name, "Time of this thread's execution: ", time.Time.Sub(end, start), colorReset)
	wg.Done()
}

func PrintColor(text string, color string) {
	fmt.Println(color, text, colorReset)
}
