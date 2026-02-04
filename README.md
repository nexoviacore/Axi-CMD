```js
const params = new URLSearchParams(window.location.search);
const q = params.get("q");

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