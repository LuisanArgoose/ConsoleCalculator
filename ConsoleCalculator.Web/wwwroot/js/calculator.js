Ext.onReady(function () {
    console.log('Calculator script loaded');
    Ext.create('Ext.form.Panel', {
        title: 'Calculator',
        width: 300,
        bodyPadding: 10,
        renderTo: Ext.getBody(),
        items: [{
            xtype: 'textfield',
            name: 'display',
            fieldLabel: 'Display',
            readOnly: true,
            id: 'display',
            fieldCls: 'calculator-display'
        }, {
            xtype: 'container',
            layout: 'vbox',
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    items: [
                        { xtype: 'button', text: '7', handler: onButtonClick },
                        { xtype: 'button', text: '8', handler: onButtonClick },
                        { xtype: 'button', text: '9', handler: onButtonClick },
                        { xtype: 'button', text: '/', handler: onButtonClick }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    items: [
                        { xtype: 'button', text: '4', handler: onButtonClick },
                        { xtype: 'button', text: '5', handler: onButtonClick },
                        { xtype: 'button', text: '6', handler: onButtonClick },
                        { xtype: 'button', text: '*', handler: onButtonClick }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    items: [
                        { xtype: 'button', text: '1', handler: onButtonClick },
                        { xtype: 'button', text: '2', handler: onButtonClick },
                        { xtype: 'button', text: '3', handler: onButtonClick },
                        { xtype: 'button', text: '-', handler: onButtonClick }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    items: [
                        { xtype: 'button', text: '0', handler: onButtonClick },
                        { xtype: 'button', text: '.', handler: onButtonClick },
                        { xtype: 'button', text: '=', handler: onEqualClick },
                        { xtype: 'button', text: '+', handler: onButtonClick }
                    ]
                }
            ]
        }]
    });

    function onButtonClick(button) {
        const display = Ext.ComponentQuery.query('#display')[0];
        display.setValue(display.getValue() + button.text);
    }

    function onEqualClick() {
        const display = Ext.ComponentQuery.query('#display')[0];
        const expression = display.getValue();

        Ext.Ajax.request({
            url: '/api/CalculatorController/calculate',
            method: 'GET',
            params: {
                expression: expression
            },
            success: function (response) {
                const result = Ext.decode(response.responseText);
                display.setValue(result);
            },
            failure: function (response) {
                display.setValue('Error');
            }
        });
    }

});