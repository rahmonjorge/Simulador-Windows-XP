package main

import (
	"fmt"
	"math/rand"
	"sync"
	"time"
)

var global1 int = 0
var global2 int = 0
var mut1 sync.Mutex
var mut2 sync.Mutex

var colorReset string = "\033[0m"

func main() {

	//colorRed := "\033[31m"
	colorGreen := "\033[32m"
	colorYellow := "\033[33m"
	colorBlue := "\033[34m"
	colorPurple := "\033[35m"
	//colorCyan := "\033[36m"
	colorWhite := "\033[37m"

	// Init variables
	iterations := 5

	// Wait Group
	var wg sync.WaitGroup
	wg.Add(iterations)

	// Threads
	go increment("[A]", colorWhite, &wg)
	go increment("[B]", colorGreen, &wg)
	go increment("[C]", colorYellow, &wg)
	go increment("[D]", colorBlue, &wg)
	go increment("[E]", colorPurple, &wg)

	// Wait
	wg.Wait()
}

func increment(name string, color string, wg *sync.WaitGroup) {
	start := time.Now()
	s1 := rand.NewSource(start.UnixNano())
	r1 := rand.New(s1)
	loop := 0
	for loop < 20 {
		choice := r1.Intn(2) + 1
		//fmt.Printf("%s %s%d\n", name, "Trying to read the global", choice)
		fmt.Println(color, name, "Trying to read the global", choice, colorReset)
		switch choice {
		case 1:
			if mut1.TryLock() {
				fmt.Println(color, name, "Reading global1 as: ", global1, colorReset)
				fmt.Println(color, mut1, colorReset)
			} else {
				fmt.Println(color, name, "Blocked, awaiting access to global1...", colorReset)
				mut1.Lock()
				fmt.Println(color, name, "Reading global1 as: ", global1, colorReset)
			}
			for i := 0; i < 5; i++ {
				global1 += 1
				time.Sleep(10 * time.Millisecond)
			}
			fmt.Println(color, name, "I changed global1 value. Now it is: ", global1, colorReset)
			mut1.Unlock()

		case 2:
			if mut2.TryLock() {
				fmt.Println(color, name, "Reading global2 as: ", global2, colorReset)
				fmt.Println(color, mut2, colorReset)
			} else {
				fmt.Println(color, name, "Blocked, awaiting access to global2...", colorReset)
				mut2.Lock()
				fmt.Println(color, name, "Reading global2 as: ", global2, colorReset)
			}
			for i := 0; i < 5; i++ {
				global2 += 1
				time.Sleep(100 * time.Millisecond)
			}
			fmt.Println(color, name, "I changed global2 value. Now it is: ", global2, colorReset)
			mut2.Unlock()
		}
		loop += 1
	}
	end := time.Now()
	fmt.Println(color, name, "Time of this thread's execution: ", time.Time.Sub(end, start), colorReset)
	wg.Done()
}
