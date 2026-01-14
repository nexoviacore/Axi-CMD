<%@ Page Language="C#" AutoEventWireup="true" CodeFile="standardPage.aspx.cs" Inherits="aspx_standardPage" %>

    <!DOCTYPE html>

    <html>

    <head runat="server">
        <meta charset="utf-8" />
        <meta name="description" content="Axpert Tstruct" />
        <meta name="keywords" content="Agile Cloud, Axpert,HMS,BIZAPP,ERP" />
        <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
        <meta name="author" content="Agile Labs" />
        <meta content="width=device-width, initial-scale=1.0" name="viewport" />
        <title>Basic Axpert Page</title>

        <script>
            var bundleCss = Array(10).fill("bundleCss").reduce((win, current, index)=>{if(!(win && win.document && win.location && win.alert && win.setInterval)){return win;}else if(win[current]){return win[current]}else if(index == (10 - 1)){return undefined;}else{return win.parent;}}, window) || [];

            bundleCss.forEach(file => document.write('<link rel="stylesheet" href="' + file + '" \/>'));
        </script>
        
        <script>
            if (!('from' in Array)) {
                // IE 11: Load Browser Polyfill
                document.write('<script src="../Js/polyfill.min.js"><\/script>');
            }
        </script>
    </head>

    <body class="content page-loading stay-page-loading d-flex flex-column flex-column-fluid fs-6 p-0"
        dir="<%=direction%>">
        <div class="h-100 d-flex flex-column">
            <div class="toolbar ms-5 pb-0 d-flex">
                <div class="d-flex flex-stack flex-wrap flex-sm-nowrap">
                    <!--begin::Info-->
                    <div class="d-flex flex-column align-items-start justify-content-center flex-wrap me-2">
                        <!--begin::Title-->
                        <h1 id="sp-caption" class="text-dark fw-bolder my-1 fs-2">
                            Axpert Standard Pages
                        </h1>
                        <!--end::Title-->
                    </div>
                    <!--end::Info-->
                </div>
            </div>

            <div class="my-6 mx-2 flex-root">
                <div class="row--- h-100 d-flex flex-column">
                    <div id="sp-header" class="mt-2 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="card h-100 shadow-sm">
                            <div class="card-header border-0---">
                                <div class="card-title">
                                    <h4 class="fw-bolder" title="List of Project Forms" contentholder>
                                        <!-- Header -->
                                    </h4>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="sp-body" class="mt-2 col-lg-12 col-md-12 col-sm-12 col-xs-12 flex-root">
                        <div class="d-flex bd-highlight gap-2 h-100">
                            <div id="sp-left" class="col-md-3">
                                <div class="card h-100 shadow-sm">
                                    <div id="sp-leftHeader" class="card-header border-0---">
                                        <div class="card-title">
                                            <h4 class="fw-bolder" title="List of Project Forms" contentholder>
                                                <!-- Left Header -->
                                            </h4>
                                        </div>
                                    </div>
                                    <div id="sp-leftBody" class="card-body px-3 pt-1 pb-3" contentholder>
                                        <!-- Left Body -->
                                        <!-- <div class="card w-100">
                                            <div class="card-body px-3 pt-1 pb-3">
                                              <h5 class="card-title">Card title</h5>
                                              <h6 class="card-subtitle mb-2 text-muted">Card subtitle</h6>
                                              <p class="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
                                            </div>
                                          </div> -->
                                    </div>
                                    <div id="sp-leftFooter" class="card-footer" contentholder>
                                        <!-- Left Footer -->
                                    </div>
                                </div>
                            </div>
                            <div id="sp-iFrame" class="flex-shrink-1 w-100">
                                <div class="card h-100 shadow-sm">
                                    <div class="card-body px-3 pt-1 pb-3 content" contentholder>
                                        <iframe id="sp-iFrameContent" class="h-100 w-100" src="" frameborder="0"></iframe>
                                    </div>
                                </div>
                            </div>
                            <div id="sp-right" class="col-md-3">
                                <div class="card h-100 shadow-sm">
                                    <div id="sp-rightHeader" class="card-header border-0---">
                                        <div class="card-title">
                                            <h4 class="fw-bolder" title="List of Project Forms" contentholder>
                                                <!-- Right Header -->
                                            </h4>
                                        </div>
                                    </div>
                                    <div id="sp-rightBody" class="card-body px-3 pt-1 pb-3" contentholder>
                                        <!-- Right Body -->
                                    </div>
                                    <div id="sp-rightFooter" class="card-footer" contentholder>
                                        <!-- Right Footer -->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="sp-footer" class="mt-2 col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="card h-100 shadow-sm">
                            <div class="card-footer" contentholder>
                                <!-- Footer -->
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <form id="form1" runat="server">
            
        </form>

        <div id="waitDiv" class="page-loader rounded-2 bg-radial-gradient">
            <div class="loader-box-wrapper d-flex bg-white p-20 shadow rounded">
                <span class="loader"></span>
            </div>
        </div>

        <script>
            var bundleJs = Array(10).fill("bundleJs").reduce((win, current, index)=>{if(!(win && win.document && win.location && win.alert && win.setInterval)){return win;}else if(win[current]){return win[current]}else if(index == (10 - 1)){return undefined;}else{return win.parent;}}, window) || [];

            bundleJs.forEach(file => document.write('<script src="' + file + '"><\/script>'));
        </script>
        <script>
            var aspBundleJs = Array(10).fill("aspBundleJs").reduce((win, current, index)=>{if(!(win && win.document && win.location && win.alert && win.setInterval)){return win;}else if(win[current]){return win[current]}else if(index == (10 - 1)){return undefined;}else{return win.parent;}}, window) || [];
            
            aspBundleJs.forEach(file => document.write('<script src="' + file + '"><\/script>'));
        </script>
    </body>

    </html>