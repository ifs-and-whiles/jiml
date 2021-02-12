# JSON inline modification language (JIML)

`JIML` is a language which allows advanced inline JSON modifications. For example

```json
{
    "a": 2 + 2 * 2,                                     //math operations

    "b": [1, 2, 3] >> (x) -> (x + 1),                   //mapping

    "c": [1, 2, 3] ?> (x) -> (x != 2),                  //filtering

    "d": [1, 2, 3] >< 0, (acc, x) -> (acc + x),         //reducing

    "e": [1, 2, 3] >> (x) -> (x + 1)
                   ?> (x) -> (x != 2)
                   >< 0, (acc, x) -> (acc + x)          //chaining operations

    "f": {
        ? true -> "a": "val a",     //if condition met then append property

        ? false -> "b": "val b",    //if not then property won't be appended

        ? false -> "c": "val c" 
        | "d": "val d",             //if - else for appending properties

        ? false -> "e": "val e" 
        |? false -> "f": "val f"
        | "g" : "val g"     //if - elseif - else for appending properties
    },

    "g": [
        ? true -> 1,        //if condition met then insert element

        ? false -> 2,       //if not then element won't be inserted

        ? false -> 3 
        | 4,                //if - else for inserting elements

        ? false -> 5
        |? false -> 6
        | 7,                //if - elseif - else for inserting elements


        ...[8, 9]                   //spreading arrays

        ? false -> ...[10, 11]      //conditional spreading arrays             
    ]
}
```

After parsing:

```json
{
    "a": 6,
    "b": [2, 3, 4],
    "c": [1, 3],
    "d": 6,
    "e": 7,
    "f": {
        "a": "val a",
        "d": "val d",
        "g": "val g"
    },
    "g": [1, 4, 7, 8, 9]
}
```

`JIML` can also accept other JSON as input for inline operations, for example:

```json
INPUT:

{
    "input": {
        "a": "val a",
        "array": [1, 2, 3, 4, 5],
        "obj": {
            "b" : {
                "c" : [{
                    "d": "val d"
                }, {
                    "e": "val e"
                }]
            }
        }
    }
}

EXPRESSION:

{
    "a": input.a,               //accessing input properties
    
    "b": input.array,           //accessing input arrays

    "c": input.obj.b.c[1].e     //accessing deeply nested value
    
    "arrays": {
        "a": input.array[0],        //accessing array elements by index

        "b": input.array[0, 2],     //picking more than one element from array by index

        "c": input.array[2:4],      //getting sub-array with range closed on both sides

        "d": input.array[:4],       //getting sub-array with range opened on the left
        
        "e": input.array[2:],       //getting sub-array with range opened on the right
        
        "f": input.array[:],        //getting sub-array with range opened on both sides

        "g": input.array[-4:-1],    //getting sub-array with negative range values 
                                    //(to count from last element of array)

        "h": input.array[           
            ? true -> 0,            // if condition met then pick element at given index

            ? false -> 1,           // if not then element won't be picked

            ? false -> 2 | 3,        // if - else for index picking

            ? false -> 2 
            |? false -> 3 
            | 4                     // if - elseif - else for index picking
        ]
    }
}
```

After parsing:

```json
{
    "a": "val a",
    "b": [1, 2, 3],
    "c": "val e",
    "arrays": {               
        "a": 0,
        "b": [1, 3],
        "c": [3, 4],
        "d": [1, 2, 3, 4],
        "e": [3, 4, 5],
        "f": [1, 2, 3, 4, 5],
        "g": [2, 3, 4],
        "h": [1, 4, 5]
    },


```

And something more complicated:

```json
INPUT:

{
    "input": [1,2,3]
}

EXPRESSION:

{
    "value": input ?> (x) -> (x != 2)       // [1, 3]
                   >> (x) -> ({
                       "a": [x, x * 2],
                       "b": [x, x + 2]
                   })                       //[{"a": [1, 2], "b": [1, 3]},
                                            // {"a": [3, 6], "b": [3, 5]}]
                   >< [], (acc, x) -> ([...acc, ...x.a, x.b[1]])      // [1, 2, 3, 3, 6, 5]
                   >< 0, (acc, x) -> (acc + x)                        // 1 + 2 + 3 + 3 + 6 + 5
                   
}

```

After parsing:

```json
{
    "value": 20
}
```

`To be continued...`