/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here.
	// For complete reference see:
    // http://docs.ckeditor.com/#!/api/CKEDITOR.config


    //config.skin = 'office2003';

    config.toolbar = 'Full';

    config.toolbarCanCollapse = true;


    // The toolbar groups arrangement, optimized for two toolbar rows.

    config.toolbar_Full = [

        ['Source', '-', 'Save', 'NewPage', 'Preview', '-', 'Templates'],

        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print', 'SpellChecker', 'Scayt'],

        ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],

        ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'],

        '/',

        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],

         ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],

         ['justifyleft', 'justifycenter', 'justifyright', 'justifyblock'],

         ['link', 'unlink', 'anchor'],

        ['image', 'flash', 'table', 'horizontalrule', 'smiley', 'specialchar', 'pagebreak'],

        '/',

         ['Styles', 'Format', 'Font', 'FontSize', 'Color'],

         ['TextColor', 'BGColor'],

    ];


    //工具栏的位置

    config.toolbarLocation = 'top';//可选：bottom

    // 编辑器的z-index值

    config.baseFloatZIndex = 10000;

    //设置快捷键
    config.keystrokes = [

       [CKEDITOR.ALT + 121 /*F10*/, 'toolbarFocus'], //获取焦点

        [CKEDITOR.ALT + 122 /*F11*/, 'elementsPathFocus'], //元素焦点

       [CKEDITOR.SHIFT + 121 /*F10*/, 'contextMenu'], //文本菜单

       [CKEDITOR.CTRL + 90 /*Z*/, 'undo'], //撤销

        [CKEDITOR.CTRL + 89 /*Y*/, 'redo'], //重做

        [CKEDITOR.CTRL + CKEDITOR.SHIFT + 90 /*Z*/, 'redo'], //

        [CKEDITOR.CTRL + 76 /*L*/, 'link'], //链接

        [CKEDITOR.CTRL + 66 /*B*/, 'bold'], //粗体

        [CKEDITOR.CTRL + 73 /*I*/, 'italic'], //斜体

        [CKEDITOR.CTRL + 85 /*U*/, 'underline'], //下划线

        [CKEDITOR.ALT + 109 /*-*/, 'toolbarCollapse']

    ]

    //设置编辑内元素的背景色的取值 plugins/colorbutton/plugin.js.

    config.colorButton_backStyle = {

        element: 'span',

        styles: { 'background-color': '#666666' }

    }

    config.colorButton_colors = '000,800000,8B4513,2F4F4F,008080,000080,4B0082,696969,B22222,A52A2A,DAA520,006400,40E0D0,0000CD,800080,808080,F00,FF8C00,FFD700,008000,0FF,00F,EE82EE,A9A9A9,FFA07A,FFA500,FFFF00,00FF00,AFEEEE,ADD8E6,DDA0DD,D3D3D3,FFF0F5,FAEBD7,FFFFE0,F0FFF0,F0FFFF,F0F8FF,E6E6FA,FFF';

    //是否在选择颜色时显示“其它颜色”选项 plugins/colorbutton/plugin.js

    config.colorButton_enableMore = false

    //区块的前景色默认值设置 plugins/colorbutton/plugin.js

    config.colorButton_foreStyle = {

        element: 'span',

        styles: { 'color': '#(color)' }

    };




















	// Remove some buttons provided by the standard plugins, which are
	// not needed in the Standard(s) toolbar.
	config.removeButtons = 'Underline,Subscript,Superscript';

	// Set the most common block elements.
	config.format_tags = 'p;h1;h2;h3;pre';

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';
};
