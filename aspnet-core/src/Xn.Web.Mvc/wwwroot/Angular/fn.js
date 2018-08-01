
app.factory('lastFistDate',function() {
  var factory = {};
  factory.last = function(y, m) {
    var lastDay = new Date(y, m, 0);
    return lastDay;
  };
  factory.first = function(y, m) {
    return new Date(y, m-1, 1);
  };
  factory.firstOrLastQui = function(y, m, b) {
    var t = m * 3;
    //console.log(t + ' ' + m);
    if (b == 0) {
      return new Date(y, t - 3, 1);
    } else {
      return new Date(y, t, 0);
    }
  };
    function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
  };
  factory.unique = function(l) {
    return  l.filter(onlyUnique);
  }
  return factory;
});
app.factory('fn', function () {
  var factory = {};
  factory.print = function(divName, className) {
    var printContents = document.getElementById(divName).innerHTML;
    var popupWin = window.open('', '_blank', 'width=300,height=300');
    popupWin.document.open();
    popupWin.document.write('<html>' +
      '<head>' +
      ' <link href="/fonts/roboto/roboto.css" rel="stylesheet"  />' +
      '<link href = "/fonts/material-icons/materialicons.css" rel = "stylesheet"  />' +
      '<link href="/lib/bootstrap/dist/css/bootstrap.css" rel="stylesheet"  />' +
      '<link href="/lib/bootstrap-select/dist/css/bootstrap-select.css" rel="stylesheet"  />' +
      '<link href="/lib/toastr/toastr.css" rel="stylesheet"  />' +
      '<link href="/lib/famfamfam-flags/dist/sprite/famfamfam-flags.css" rel="stylesheet"  />' +
      '<link href="/lib/font-awesome/css/font-awesome.css" rel="stylesheet"  />' +
      '<link href="/lib/Waves/dist/waves.css" rel="stylesheet"  />' +
      '<link href="/lib/animate.css/animate.css" rel="stylesheet"  />' +
      ' <link href="/css/materialize.css" rel="stylesheet"  />' +
      ' <link href="/css/style.css" rel="stylesheet" >' +
      ' <link href="/css/styleXns.css" rel="stylesheet" >' +
      '<link href="/css/testPrint.css" rel="stylesheet" >' +
      ' <link href="/css/themes/all-themes.css" rel="stylesheet"  />' +
      ' <link href="/view-resources/Views/Shared/_Layout.css" rel="stylesheet"  />' +
      ' <link href="/js/table-angularjs/jquery.dataTables.min.css" rel="stylesheet" >' +
      ' <link href="/scss/snackbar.css" rel="stylesheet"  />' +
      ' <link href="/scss/' +
      className +
      '.css" rel="stylesheet"  />' +
      '</head>' +
      '<body onload="window.print()">' +
      printContents +
      '</body>' +
      '<script src="/js/table-angularjs/jquery.dataTables.min.js"></script>' +
      '</html>');
    popupWin.document.close();
  };
  function onlyUnique(value, index, self) {
    return self.indexOf(value) === index;
  };

  factory.unique = function(l) {
    return l.filter(onlyUnique);
  };
  return factory;
});

