module.exports = {
  base: '/',
  locales: {
    '/': {
      lang: 'zh-CN',
      title: 'DDTV',
      keywords: 'DDTV bilibili bili live 播放器 录播机 B站录播机 bilibili自动录制',
      description: 'DDTV-你的地表最强B站播放器。一个可进行B站直播多窗口观看、开播提醒、自动录制、直播状态查看、跨平台部署的绿色工具。'
    }
  },
  head: [
    ['meta', { name: 'keywords', content: 'DDTV,bilibili,bili,live,播放器,录播机,B站录播机,bilibili自动录制' }]
  ],
  themeConfig: {
    algolia: {},
    logo: '/DDTV.png',
    repo: 'CHKZL/DDTV',
    docsDir: 'Doc/docs',
    docsBranch: 'master',
    editLinks: true,
    editLinkText: '在 GitHub 上编辑此页',
    lastUpdated: '上次更新',
    smoothScroll: true,
    nav: [
      { text: '主页', link: '/' },
      { text: '安装', link: '/install/' },
      { text: '配置说明', link: '/config/' },
      { text: '关于项目', link: '/about/' }
    ],
    sidebar: {
      '/install/': [
        {
          title: '安装',
          collapsable: false,
          sidebar: 'auto',
          children: [
            '',
            'DDTV_Destkop(Windows)',
            'DDTV_Server(Linux)',
            'DDTV_Server(Windows)',
            'DDTV_Client(Windows)', 
            'Docker',
            'WEBUI'
          ]
        }
      ],
      '/config/': [
        {
          title: '配置说明',
          collapsable: false,
          sidebar: 'auto',
          children: [
            '',
            'DDTV_Config',
            'RoomListConfig.json',
          ]
        }
      ], '/about/': [
        {
          title: '关于项目',
          collapsable: false,
          sidebar: 'auto',
          children: [
            '',
            '免责声明'
          ]
        }
      ]
    }
  },
}
