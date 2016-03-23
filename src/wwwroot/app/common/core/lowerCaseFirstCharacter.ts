/// <reference path="../../typings/_all.d.ts" />
module Antares.Core{
    export function lowerCaseFirstCharacter(item:string) {
        var itemArray = item.split("");
        itemArray[0] = itemArray[0].toLowerCase()[0];
        return itemArray.join("");
    }
}