//const API_AXLIST = "http://localhost:5000/api/v1/AxList"; 
//const API_ARM_SIGNIN = "http://localhost:5000/api/v1/Signin";

//let signingInPromise = null;

//const TOKEN_TTL_MS = 4 * 60 * 60 * 1000;
////const TOKEN_TTL_MS = 1 * 60 * 1000; // 1 minute (TEST ONLY)

//export async function ensureSignedIn(appname) {
//    if (isAuthValid()) return;

//    if (!signingInPromise) {
//        signingInPromise = signIn(appname)
//            .finally(() => signingInPromise = null);
//    }

//    await signingInPromise;
//}

///* ===============================
//   TOAST HELPER
//=============================== */
//function showToast(message, duration = 5000) {
//    // 1. Create Element
//    const toast = document.createElement("div");
//    toast.textContent = message;

//    // 2. Style it (Inline styles for simplicity, or use a CSS class)
//    Object.assign(toast.style, {
//        position: "fixed",
//        bottom: "20px",
//        right: "20px",
//        backgroundColor: "#ef4444", // Red-500
//        color: "white",
//        padding: "12px 24px",
//        borderRadius: "8px",
//        boxShadow: "0 4px 6px rgba(0,0,0,0.1)",
//        zIndex: "10000",
//        fontFamily: "sans-serif",
//        fontSize: "14px",
//        opacity: "0",
//        transition: "opacity 0.3s ease-in-out"
//    });

//    document.body.appendChild(toast);

//    // 3. Show Animation
//    requestAnimationFrame(() => {
//        toast.style.opacity = "1";
//    });

//    // 4. Auto-Hide Logic
//    setTimeout(() => {
//        toast.style.opacity = "0";
//        setTimeout(() => {
//            if (document.body.contains(toast)) {
//                document.body.removeChild(toast);
//            }
//        }, 300); // Wait for fade out
//    }, duration);
//}

//function saveAuth(accessToken, armSessionId) {
//    const expiresAt = Date.now() + TOKEN_TTL_MS;

//    localStorage.setItem("arm_accessToken_v1", accessToken);
//    localStorage.setItem("arm_armSessionId_v1", armSessionId);
//    localStorage.setItem("arm_auth_expiresAt_v1", expiresAt.toString());
//}

//function isAuthValid() {
//    const token = localStorage.getItem("arm_accessToken_v1");
//    const session = localStorage.getItem("arm_armSessionId_v1");
//    const expiresAt = Number(localStorage.getItem("arm_auth_expiresAt_v1"));

//    if (!token || !session || !expiresAt) return false;

//    if (Date.now() >= expiresAt) {
//        clearAuth();
//        return false;
//    }

//    return true;
//}

//function clearAuth() {
//    localStorage.removeItem("arm_accessToken_v1");
//    console.log("Arm accesstoken removed........");
//    localStorage.removeItem("arm_armSessionId_v1");
//    console.log("Arm session id removed..........");
//    localStorage.removeItem("arm_auth_expiresAt_v1");
//    console.log("Arm expiry removed.........");
//}



//export async function signIn(appname) {
//    console.log("Sign in called.........")

//    try {
//        //const appname = localStorage.getItem("arm_appname_v1"); 
//        const requestBody = {
//            //appname: "pgbase114", //agileerpbaselocal // orclbase114local //ax114 //ghcmdev
//            appname: appname,
//            UserName: "admin",  //mohan // salesexecutive//indiauser //admin //laksh@transper.com
//            password: "22723bbd4217a0abf6d3e68073c7603d", //827ccb0eea8a706c4c34a16891f84e7b //22723bbd4217a0abf6d3e68073c7603d //cb636c00783cdf430eedd449fcfd10c3// //827ccb0eea8a706c4c34a16891f84e7b
//            Language: "English",
//            SessionId: "12345",
//            Globalvars: true, //true
//            ClearPreviousSession: true,//true
//            trace: true
//        }

//        //const cachedAccessToken = localStorage.getItem("arm_access_token");

//        //if (accessToken) {
//        //    return JSON.parse(cached);
//        //}



//        const res = await fetch(`${API_ARM_SIGNIN}`, {
//            method: "POST",
//            headers: {
//                "Content-Type": "application/json",
//                //"Authorization": `Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkFSTS1JTlRFUk5BTC0xNzBCMzYwQ0VDMTkwNjk5MkEzMjhSUzI1NiJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJzaWQiOiJBUk0tcGdiYXNlMTE0LTU3MDc0YjAwLTg4NWUtNGQ0Mi1hYjUyLTY4NWI4OWI0MDc2MiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiQVJNLXBnYmFzZTExNC01NzA3NGIwMC04ODVlLTRkNDItYWI1Mi02ODViODliNDA3NjIiLCJuYmYiOjE3NjYyMjgxMjQsImV4cCI6MTc2NjIzMTcyNCwiaXNzIjoiQXhwZXJ0IC0gQVJNIiwiYXVkIjoiQXhwZXJ0IC0gQVJNIn0.c2Ju3kk5mxnAlCULdxoqgRKqc2SRbh-mBQOvkuvMmBE`
//            },
//            body: JSON.stringify(requestBody)

//        });

//        if (res.ok) {

//            const dataObj = await res.json();
//            //if (dataObj && dataObj.result && dataObj.result.data[0]) {
//            const accessToken = dataObj.result.token;
//            const armSessionId = dataObj.result.ARMSessionId;
//            saveAuth(accessToken, armSessionId); 
//            console.log("ARM Sign in successfull"); 


//        }

       
            
           
//        //}
//        //return [];



//    } catch (err) {
//        console.log(err);
//        return [];
//    }
//}

//export async function getTStructList() {
    
//    try {
//        //await signIn(); 
//        const accessToken = localStorage.getItem("arm_accessToken_v1");
//        console.log(accessToken); 

//        const armSessionId = localStorage.getItem("arm_armSessionId_v1");
//        console.log(accessToken); 




//        const requestBody = {
//            //ARMSessionId: "ARM-pgbase114-57074b00-885e-4d42-ab52-685b89b40762",
//            ARMSessionId: armSessionId,
//            action: "view",
//            ADSNames: [
//                "TstructList"
//            ],
//            trace: true
//        }

//        const cached = localStorage.getItem("axi_tstructList_v1");

//        if (cached) {
//            return JSON.parse(cached); 
//        }



//        const res = await fetch(`${API_AXLIST}`, {
//            method: "POST",
//            headers: {
//                "Content-Type": "application/json",
//                //"Authorization": `Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkFSTS1JTlRFUk5BTC0xNzBCMzYwQ0VDMTkwNjk5MkEzMjhSUzI1NiJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJzaWQiOiJBUk0tcGdiYXNlMTE0LTU3MDc0YjAwLTg4NWUtNGQ0Mi1hYjUyLTY4NWI4OWI0MDc2MiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiQVJNLXBnYmFzZTExNC01NzA3NGIwMC04ODVlLTRkNDItYWI1Mi02ODViODliNDA3NjIiLCJuYmYiOjE3NjYyMjgxMjQsImV4cCI6MTc2NjIzMTcyNCwiaXNzIjoiQXhwZXJ0IC0gQVJNIiwiYXVkIjoiQXhwZXJ0IC0gQVJNIn0.c2Ju3kk5mxnAlCULdxoqgRKqc2SRbh-mBQOvkuvMmBE`
//                "Authorization": `Bearer ${accessToken}`


//            },
//            body: JSON.stringify(requestBody)

//        }); 

//        const dataObj = await res.json(); 
//        if (dataObj && dataObj.result && dataObj.result.data[0]) {
//            const list = dataObj.result.data[0].data;
//            localStorage.setItem("axi_tstructList_v1", JSON.stringify(list));
//            return list;
//        }
//        return [];

        

//          } catch (err) {
//        console.log(err);
//        return []; 
//    }
//}

//export async function getAdsList() {

//    try {
//        //await signIn();
//        const accessToken = localStorage.getItem("arm_accessToken_v1");
//        console.log(accessToken);

//        const armSessionId = localStorage.getItem("arm_armSessionId_v1");
//        console.log(accessToken); 
//        const requestBody = {
//            ARMSessionId: armSessionId,
//            action: "view",
//            ADSNames: [
//                "AdsList"
//            ],
//            trace: true
//        }

//        const cached = localStorage.getItem("axi_adsList_v1");

//        if (cached) {
//            return JSON.parse(cached);
//        }



//        const res = await fetch(`${API_AXLIST}`, {
//            method: "POST",
//            headers: {
//                "Content-Type": "application/json",
//                //"Authorization": `Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkFSTS1JTlRFUk5BTC0xNzBCMzYwQ0VDMTkwNjk5MkEzMjhSUzI1NiJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJzaWQiOiJBUk0tcGdiYXNlMTE0LTU3MDc0YjAwLTg4NWUtNGQ0Mi1hYjUyLTY4NWI4OWI0MDc2MiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiQVJNLXBnYmFzZTExNC01NzA3NGIwMC04ODVlLTRkNDItYWI1Mi02ODViODliNDA3NjIiLCJuYmYiOjE3NjYyMjgxMjQsImV4cCI6MTc2NjIzMTcyNCwiaXNzIjoiQXhwZXJ0IC0gQVJNIiwiYXVkIjoiQXhwZXJ0IC0gQVJNIn0.c2Ju3kk5mxnAlCULdxoqgRKqc2SRbh-mBQOvkuvMmBE`
//                "Authorization": `Bearer ${accessToken}`

//            },
//            body: JSON.stringify(requestBody)

//        });

//        const dataObj = await res.json();
//        if (dataObj && dataObj.result && dataObj.result.data[0]) {
//            const list = dataObj.result.data[0].data;
//            localStorage.setItem("axi_adsList_v1", JSON.stringify(list));
//            return list;
//        }
//        return [];



//    } catch (err) {
//        console.log(err);
//        return [];
//    }
//}

//export async function getFieldList(tstructname) {

//    try {
//        console.log("FieldList called")
//        //return [];
//        //await signIn();
//        const accessToken = localStorage.getItem("arm_accessToken_v1");
//        console.log(accessToken);

//        const armSessionId = localStorage.getItem("arm_armSessionId_v1");
//        console.log(accessToken);
//        const requestBody = {
//            ARMSessionId: armSessionId,
//            action: "view",
//            ADSNames: [
//                "AxpFieldList"
//            ],
//            trace: true,
//            sqlparams: {
//                tstructname: tstructname
//            }
//        }

//        const cached = localStorage.getItem(`axi_fieldList_${tstructname}_v1`);

//        if (cached) {
//            return JSON.parse(cached);
//        }



//        const res = await fetch(`${API_AXLIST}`, {
//            method: "POST",
//            headers: {
//                "Content-Type": "application/json",
//                //"Authorization": `Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkFSTS1JTlRFUk5BTC0xNzBCMzYwQ0VDMTkwNjk5MkEzMjhSUzI1NiJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJzaWQiOiJBUk0tcGdiYXNlMTE0LTU3MDc0YjAwLTg4NWUtNGQ0Mi1hYjUyLTY4NWI4OWI0MDc2MiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiQVJNLXBnYmFzZTExNC01NzA3NGIwMC04ODVlLTRkNDItYWI1Mi02ODViODliNDA3NjIiLCJuYmYiOjE3NjYyMjgxMjQsImV4cCI6MTc2NjIzMTcyNCwiaXNzIjoiQXhwZXJ0IC0gQVJNIiwiYXVkIjoiQXhwZXJ0IC0gQVJNIn0.c2Ju3kk5mxnAlCULdxoqgRKqc2SRbh-mBQOvkuvMmBE`
//                "Authorization": `Bearer ${accessToken}`

//            },
//            body: JSON.stringify(requestBody)

//        });

//        const dataObj = await res.json();
//        if (dataObj && dataObj.result && dataObj.result.data[0]) {
//            const list = dataObj.result.data[0].data;
//            localStorage.setItem(`axi_fieldList_${tstructname}_v1`, JSON.stringify(list));
//            return list;
//        }
//        return [];



//    } catch (err) {
//        console.log(err);
//        return [];
//    }
//}

//export async function getFieldValueList() {

//    try {
//        //await signIn();
//        console.log("FieldValueList called")
//        return [];
//        const accessToken = localStorage.getItem("arm_accessToken_v1");
//        console.log(accessToken);

//        const armSessionId = localStorage.getItem("arm_armSessionId_v1");
//        console.log(accessToken);
//        const requestBody = {
//            ARMSessionId: armSessionId,
//            action: "view",
//            ADSNames: [
//                "AdsList"
//            ],
//            trace: true
//        }

//        const cached = localStorage.getItem("axi_adsList_v1");

//        if (cached) {
//            return JSON.parse(cached);
//        }



//        const res = await fetch(`${API_AXLIST}`, {
//            method: "POST",
//            headers: {
//                "Content-Type": "application/json",
//                //"Authorization": `Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkFSTS1JTlRFUk5BTC0xNzBCMzYwQ0VDMTkwNjk5MkEzMjhSUzI1NiJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJzaWQiOiJBUk0tcGdiYXNlMTE0LTU3MDc0YjAwLTg4NWUtNGQ0Mi1hYjUyLTY4NWI4OWI0MDc2MiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiQVJNLXBnYmFzZTExNC01NzA3NGIwMC04ODVlLTRkNDItYWI1Mi02ODViODliNDA3NjIiLCJuYmYiOjE3NjYyMjgxMjQsImV4cCI6MTc2NjIzMTcyNCwiaXNzIjoiQXhwZXJ0IC0gQVJNIiwiYXVkIjoiQXhwZXJ0IC0gQVJNIn0.c2Ju3kk5mxnAlCULdxoqgRKqc2SRbh-mBQOvkuvMmBE`
//                "Authorization": `Bearer ${accessToken}`

//            },
//            body: JSON.stringify(requestBody)

//        });

//        const dataObj = await res.json();
//        if (dataObj && dataObj.result && dataObj.result.data[0]) {
//            const list = dataObj.result.data[0].data;
//            localStorage.setItem("axi_adsList_v1", JSON.stringify(list));
//            return list;
//        }
//        return [];



//    } catch (err) {
//        console.log(err);
//        return [];
//    }
//}

///* Generic get List function */
//export async function getList(axDatasourceName, paramValuesCsv = "") {
//    try {
//        await ensureSignedIn(); 
//        if (!axDatasourceName) {
//            throw new Error("axDatasourceName is required");
//        }

//        const accessToken = localStorage.getItem("arm_accessToken_v1");
//        const armSessionId = localStorage.getItem("arm_armSessionId_v1");

//        if (!accessToken || !armSessionId) {
//            console.error("Missing auth/session data");
//            return [];
//        }

//        // ---- Build sqlparams dynamically ----
//        const sqlparams = {};
//        const normalizedParams = [];

//        if (paramValuesCsv && typeof paramValuesCsv === "string") {
//            const values = paramValuesCsv
//                .split(",")
//                .map(v => v.trim())
//                .filter(Boolean);

//            values.forEach((value, index) => {
//                const key = `param${index + 1}`;
//                sqlparams[key] = value;
//                normalizedParams.push(`${key}:${value}`);
//            });
//        }

//        // ---- Stable cache key ----
//        const cacheKey = `axi_${axDatasourceName}_${normalizedParams.join("|")}_v1`;

//        const cached = localStorage.getItem(cacheKey);
//        if (cached) {
//            return JSON.parse(cached);
//        }

//        const requestBody = {
//            ARMSessionId: armSessionId,
//            action: "view",
//            ADSNames: [axDatasourceName],
//            trace: true,
//            sqlparams
//        };

//        const res = await fetch(API_AXLIST, {
//            method: "POST",
//            headers: {
//                "Content-Type": "application/json",
//                Authorization: `Bearer ${accessToken}`
//            },
//            body: JSON.stringify(requestBody)
//        });

//        //if (!res.ok) {
//        //    console.log("API error:", res.status, res.statusText);
//        //    showToast(`Something went wrong ${res.statusText}`); 
//        //    return [];
//        //}

       




//        console.log(`STatus : ${res.status}`)

//        const dataObj = await res.json();
//        if (res.status === 206) {
//            const errorMsg = dataObj?.result?.data?.[0]?.error;

//            if (errorMsg) {
//                console.error("API Partial Error:", errorMsg);
//                showToast(`Error: ${errorMsg}`);
//                return []; 
//            }
//        }
//        const list = dataObj?.result?.data?.[0]?.data ?? [];

//        localStorage.setItem(cacheKey, JSON.stringify(list));
//        return list;

//    } catch (err) {
//        //console.error("getList failed:", err);
//        //showToast(`Error: ${err}`); 
//        return [];
//    }
//}

//export function logAuthExpiry() {
//    const expiresAt = Number(localStorage.getItem("arm_auth_expiresAt_v1"));

//    if (!expiresAt) {
//        console.log("Auth expiry: NOT SET");
//        return;
//    }

//    const remainingMs = expiresAt - Date.now();

//    if (remainingMs <= 0) {
//        console.log("Auth expired");
//        return;
//    }

//    const minutes = Math.floor(remainingMs / 60000);
//    const seconds = Math.floor((remainingMs % 60000) / 1000);

//    console.log(`Auth expires in ${minutes}m ${seconds}s`);
//}



//window.ensureSignedIn = ensureSignedIn;
//window.getTStructList = getTStructList;
//window.getAdsList = getAdsList;
//window.signIn = signIn;
//window.getFieldList = getFieldList;
//window.getFieldValueList = getFieldValueList;
//window.getList = getList;
//window.logAuthExpiry = logAuthExpiry;