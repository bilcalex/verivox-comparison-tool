const uri = "api/compare/";

function getData() {
  $.ajax({
    type: "GET",
    url: uri + document.getElementById('get-comparison').value,
    cache: false,
    success: function(data) {
      const tBody = $("#comparisons");

      $(tBody).empty();

      $.each(data, function(key, item) {
        const tr = $("<tr></tr>")
          .append($("<td></td>").text(item.type == 0 ? "Basic Tariff" : "Packaged Tariff"))
          .append($("<td></td>").text(item.annualCost))
          
        tr.appendTo(tBody);
      });
    }
  });
}