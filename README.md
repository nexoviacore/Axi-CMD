Encoding code snippet:

```js
const payload = {

            filters: filters
        }

        const encodedFilterQuery = btoa(JSON.stringify(payload));

        let targetUrl = "../CustomPages/Smartview_table_1769088257557.html";
        targetUrl += `?ads=${encodeURIComponent(adsName)}`;
        targetUrl += "&load=1769601086182";
        targetUrl += `&filter=${encodedFilterQuery}`;
```


Decoding code snippet
```js
const params = new URLSearchParams(window.location.search);
const q = params.get("filter");

if (!q) {
    console.warn("No SmartView payload found");
} else {
    try {
        const { ads, filters } = JSON.parse(atob(q));
        console.log("SmartView ADS:", ads);
        console.log("SmartView Filters:", filters);

        // TODO:
        // use `ads` and `filters` to build your SQL / ADS query
    } catch (e) {
        console.error("Invalid SmartView payload", e);
    }
}
```


## Pending and Discussion Points

- Enable Run command for Entity page, Entity Form Page and Design mode screen

- Code Review and Code Optimization. 

```js
let value; 

if (typeof value !== Number) {
    return; 
}
```


## Run Button / Script from Command Line
View inbox buttons need to be handled. -> completed 
Apply filter added to run the command from the listing page should be ignored (check and ignore it).
Add GO [Ctrl + Enter] option in AutoComplete
For the first token, GO (Ctrl + Enter) should not be listed in view mode.
Edit TStruct → if only the edit form is provided, open the form in new mode.
Add [Ctrl + Enter] support.
Similar to View, Config also needs to be handled.
Backlog
Axi command – run search (TStruct / iView) needs enhancements.
If the caption contains parentheses, consider only the last occurrence of the opening and closing parentheses.