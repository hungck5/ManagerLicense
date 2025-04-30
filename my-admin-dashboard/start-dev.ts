// start-dev.ts
import { spawn } from 'child_process';

function run(command: string, args: string[]) {
  const proc = spawn(command, args, { stdio: 'inherit', shell: true });
  
  proc.on('close', (code) => {
    console.log(`${command} exited with code ${code}`);
  });
}

// Chạy 2 tiến trình song song
run('pnpm', ['run', 'dev-http']);
run('pnpm', ['run', 'dev-https']);
