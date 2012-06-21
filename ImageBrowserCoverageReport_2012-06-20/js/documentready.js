$(document).ready(function () {
    $("#treeDiv").dynatree({
        keyboard: true,    
        imagePath: "ImageBrowserCoverageReport_2012-06-20/images/",
        onPostInit: function (isReloading, isError) {
            var rootNode = $("#treeDiv").dynatree("getRoot");
            rootNode.data.childIds = [0];
            loadChildren(rootNode);
        },

        onCustomRender: function(node) {
                return createNodePresentation(node.data.title, node.data.percent, node.data.clickable);
        },

        onActivate: function (node) {
            // Use our custom attribute to load the new target content:
            if (node.data.url != "")
                $("#sourceViewer").attr("src", node.data.url);
            else
                $("#sourceViewer").attr("src", blankSource);
        },      

        onLazyRead: function(node) {
               loadChildren(node);
               return true;
              }
    });

    var myLayout = $('#splitters').layout({
      onresize: resizeDynamicContent,
      south__closable: false,
      south__resizable: false,
      south__spacing_open: 0,
      north__closable: false,
      north__resizable: false,
      north__spacing_open: 0
   });
   
   var width = $('body')[0].clientWidth - 350;
   if (width > 0)
      myLayout.sizePane("east", $('body')[0].clientWidth - 350);
        
   resizeDynamicContent();
   
})