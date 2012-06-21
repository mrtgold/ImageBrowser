function loadChildren(node) {
    if (!node.data.childIds)
        return;

      $.each(node.data.childIds, function (i, childId) {
        executeWithNode(childId, function (data) {
          var hasChildren = data.c && data.c.length > 0;
          var sourceUrl = data.f ? (sourceReferencePrefix + data.f + ".html#src" + data.f + "." + (data.k ? "dccv" : "dcuc") + "." + data.y + "." + data.x) : "";
          var nodeTypeImageName = nodeTypeImageNames[data.i];
          node.addChild({ key: childId, title: data.n, percent: data.p, childIds: data.c, isLazy: hasChildren, isFolder: false, icon: nodeTypeImageName == "" ? false : nodeTypeImageName, url: sourceUrl, clickable: sourceUrl != "" });
        });
      });
}

function createNodePresentation(name, percent, clickable) {
    var renderedNode = "<img src='ImageBrowserCoverageReport_2012-06-20/images/" + percent + ".png' class='percent-image'>";
    var clazz = clickable ? "clickable-coverage-node" : "coverage-node";
    renderedNode += "<a href='#' class='" + clazz + "'>" + name + "</a>";
    return renderedNode;
}

function executeWithNode(id, handler) {

  var blockIndex = div(id, blockSize);
  var block = coverageData[blockIndex];
  if (!block)
    return;

  var itemIndex = id % blockSize;
  var item = block[itemIndex];
  if (!item)
    return;
  
  handler(item);
}

function div(x, y) {
    var d = x / y;
    // since js has no integer division
    if(d >= 0)
      return Math.floor(d);
    else
      return Math.ceil(d);
}