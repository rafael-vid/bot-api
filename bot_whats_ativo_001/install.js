var Service = require('node-windows').Service;

// Create a new service object
var svc = new Service({
  name:'Bot_whats_ativo_001',
  description: 'Bot_whats_ativo_001',
  script: './out/index.js',
  nodeOptions: [
    '--harmony',
    '--max_old_space_size=4096'
  ]
  //, workingDirectory: '...'
  //, allowServiceLogon: true
});

// Listen for the "install" event, which indicates the
// process is available as a service.
svc.on('install',function(){
console.log('install')
  svc.start();
});

svc.install();