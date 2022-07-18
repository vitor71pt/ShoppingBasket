#!/bin/bash
run_test()
{
    echo $1;

    dotnet run $1

    read  -n 1 -p "Press any key:" mainmenuinput
}

cd ../../src/ShoppingBasket.Console

run_test 'soup bread milk'

run_test 'soup bread milk soup apples'

run_test 'carrots'
