﻿if (!String.prototype.supplant) {
    String.prototype.supplant = function(o) {
        return this.replace(/{([^{}]*)}/g,
            function(a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    }
}

$(function() {
    var ticker=$.connection.stockTickerMini,
         up = '▲',
        down = '▼',
        $stockTable=$("#stockTable"),
        $stockTableBody=$stockTable.find("tbody"),
        rowTemplate = '<tr data-symbol="{Symbol}"><td>{Symbol}</td><td>{Price}</td><td>{DayOpen}</td><td>{Direction} {Change}</td><td>{PercentChange}</td></tr>'

    function formatStock(stock) {
        return $.extend(stock, {
            Price: stock.Price.toFixed(2),
            PercentChange: (stock.PercentChange * 100).toFixed(2) + '%',
            Direction: stock.Change === 0 ? '' : stock.CHANGE >= 0 ? up : down
        });
    }

    function init() {
        ticker.server.getAllStocks().done(function(stocks) {
            $stockTableBody.empty();
            $.each(stocks, function() {
                var stock = formatStock(this);
                $stockTableBody.append(rowTemplate.supplant(stock));
            });
        });
    }
})