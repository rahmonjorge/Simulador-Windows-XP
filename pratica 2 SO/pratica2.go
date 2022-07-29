package main

import (
	"fmt"
	"sync"
	"time"
)

var global1 int
var global2 int

var (
	lock sync.Mutex
)

func main() {
	// Init variables
	global1 = 0
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
	fmt.Println(name, "Reading globals as: ", global1, global2)
	global1 = global1 + 1
	global2 = global2 + 1
	fmt.Println(name, "I changed global's value. now they are: ", global1, global2)
	wg.Done()
	end := time.Now()
	fmt.Println(name, "Time of this thread's execution: ", time.Time.Sub(end, start))
}
