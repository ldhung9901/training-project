import expressions from "angular-expressions";
import { assign } from "lodash";


expressions.filters.lower =  (input)=> {
    if (!input) return input;
    return input.toLowerCase();
}
export const angularParser= (tag) =>{
    if (tag === '.') {
        return {
            get: function (s) { return s; }
        };
    }
    const expr = expressions.compile(
        tag.replace(/(’|‘)/g, "'").replace(/(“|”)/g, '"')
    );
    return {
        get: function (scope, context) {
            let obj = {};
            const scopeList = context.scopeList;
            const num = context.num;
            for (let i = 0, len = num + 1; i < len; i++) {
                obj = assign(obj, scopeList[i]);
            }
            return expr(scope, obj);
        }
    };
}

export const nullGetter = (part) =>{
    /*
        If the template is {#users}{name}{/} and a value is undefined on the
        name property:

        - part.value will be the string "name"
        - scopeManager.scopePath will be ["users"] (for nested loops, you would have multiple values in this array, for example one could have ["companies", "users"])
        - scopeManager.scopePathItem will be equal to the array [2] if
          this happens for the third user in the array.
        - part.module would be empty in this case, but it could be "loop",
          "rawxml", or or any other module name that you use.
    */

    if (!part.module) {
        // part.value contains the content of the tag, eg "name" in our example
        // By returning '{' and part.value and '}', it will actually do no replacement in reality. You could also return the empty string if you prefered.
        return "";
    }
    if (part.module === "rawxml") {
        return "";
    }
    return "";
}
