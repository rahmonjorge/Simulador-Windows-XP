package main

import (
	"fmt"
	"math/rand"
	"sync"
	"time"
)

var global1 int
var global2 int
var mut1 sync.Mutex
var mut2 sync.Mutex

func main() {
	// Init variables
	global1 = 0
	global2 = 0
	iterations := 5

	// Wait Group
	var wg sync.WaitGroup
	wg.Add(iterations)

	// Threads
	go increment("[A]", &wg)
	go increment("[B]", &wg)
	go increment("[C]", &wg)
	go increment("[D]", &wg)
	go increment("[E]", &wg)

	// Wait
	wg.Wait()
}

func increment(name string, wg *sync.WaitGroup) {
	start := time.Now()
	s1 := rand.NewSource(start.UnixNano())
	r1 := rand.New(s1)
	loop := 0
	for loop < 25 {
		choice := r1.Intn(2) + 1
		fmt.Printf("%s %s%d\n", name, "Trying to read the global", choice)
		switch choice {
		case 1:
			if mut1.TryLock() {
				fmt.Println(name, "Reading global1 as: ", global1)
				fmt.Println(mut1)
			} else {
				fmt.Println(name, "Blocked, awaiting access to global1...")
				mut1.Lock()
				fmt.Println(name, "Reading global1 as: ", global1)
			}
			for i := 0; i < 5; i++ {
				global1 += 1
				time.Sleep(10 * time.Millisecond)
			}
			fmt.Println(name, "I changed global1 value. Now it is: ", global1)
			mut1.Unlock()

		case 2:
			if mut2.TryLock() {
				fmt.Println(name, "Reading global2 as: ", global2)
				fmt.Println(mut2)
			} else {
				fmt.Println(name, "Blocked, awaiting access to global2...")
				mut2.Lock()
				fmt.Println(name, "Reading global2 as: ", global2)
			}
			for i := 0; i < 5; i++ {
				global2 += 1
				time.Sleep(100 * time.Millisecond)
			}
			fmt.Println(name, "I changed global2 value. Now it is: ", global2)
			mut2.Unlock()
		}
		loop += 1
	}
	end := time.Now()
	fmt.Println(name, "Time of this thread's execution: ", time.Time.Sub(end, start))
	wg.Done()
}
