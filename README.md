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