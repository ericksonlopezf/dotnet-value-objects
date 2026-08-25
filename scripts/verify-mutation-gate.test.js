// Copyright © Erickson Lopez. MIT License.
const assert = require('assert');
const {
  loadThresholds,
  parseScoreFromDescription,
  evaluateScore,
  verifyMutationGate,
  MAX_REPORT_AGE_DAYS
} = require('./verify-mutation-gate');

console.log('Running tests for verify-mutation-gate.js...\n');

// Test 1: loadThresholds from stryker-config.json
{
  const thresholds = loadThresholds();
  assert.strictEqual(thresholds.high, 100, 'Threshold high should be 100');
  assert.strictEqual(thresholds.low, 98, 'Threshold low should be 98');
  assert.strictEqual(thresholds.break, 95, 'Threshold break should be 95');
  console.log('✅ Test 1 Passed: loadThresholds loads correct values from stryker-config.json');
}

// Test 2: parseScoreFromDescription
{
  assert.strictEqual(parseScoreFromDescription('Stryker: 100% (240/240 killed) - ✅ HIGH'), 100);
  assert.strictEqual(parseScoreFromDescription('Stryker: 98.5% (200/203 killed) - 🟡 LOW'), 98.5);
  assert.strictEqual(parseScoreFromDescription('Stryker: 95.0% - 🟠 WARNING'), 95.0);
  assert.strictEqual(parseScoreFromDescription('Stryker: 94.2% - ❌ FAILED'), 94.2);
  assert.strictEqual(parseScoreFromDescription(null), null);
  assert.strictEqual(parseScoreFromDescription('No percentage here'), null);
  console.log('✅ Test 2 Passed: parseScoreFromDescription correctly extracts numeric percentage');
}

// Test 3: evaluateScore
{
  const thresholds = { high: 100, low: 98, break: 95 };

  const resHigh = evaluateScore(100, thresholds);
  assert.strictEqual(resHigh.status, '✅ HIGH');
  assert.strictEqual(resHigh.passedBreak, true);

  const resLow = evaluateScore(98.5, thresholds);
  assert.strictEqual(resLow.status, '🟡 LOW');
  assert.strictEqual(resLow.passedBreak, true);

  const resWarn = evaluateScore(96.0, thresholds);
  assert.strictEqual(resWarn.status, '🟠 WARNING');
  assert.strictEqual(resWarn.passedBreak, true);

  const resBreakExact = evaluateScore(95.0, thresholds);
  assert.strictEqual(resBreakExact.status, '🟠 WARNING');
  assert.strictEqual(resBreakExact.passedBreak, true);

  const resFail = evaluateScore(94.9, thresholds);
  assert.strictEqual(resFail.status, '❌ FAILED');
  assert.strictEqual(resFail.passedBreak, false);

  console.log('✅ Test 3 Passed: evaluateScore correctly categorizes scores and break gate');
}

// Test 4: verifyMutationGate with mock direct target SHA
(async () => {
  let failed = false;
  const mockContext = {
    repo: { owner: 'ericksonlopezf', repo: 'dotnet-value-objects' },
    sha: 'abc1234567890'
  };

  const freshDate = new Date().toISOString();

  const mockGithub = {
    rest: {
      repos: {
        getCombinedStatusForRef: async ({ ref }) => {
          if (ref === 'abc1234567890') {
            return {
              data: {
                statuses: [
                  {
                    context: 'mutation-testing/stryker',
                    state: 'success',
                    description: 'Stryker: 100% (240/240 killed) - ✅ HIGH',
                    updated_at: freshDate,
                    target_url: 'https://github.com/ericksonlopezf/dotnet-value-objects/actions/runs/12345'
                  }
                ]
              }
            };
          }
          return { data: { statuses: [] } };
        }
      }
    }
  };

  const mockCore = {
    setFailed: () => { failed = true; }
  };

  await verifyMutationGate({ github: mockGithub, context: mockContext, core: mockCore });
  assert.strictEqual(failed, false, 'Should pass for 100% score on target commit');
  console.log('✅ Test 4 Passed: verifyMutationGate passes for valid target commit status');
})()
.then(async () => {
  // Test 5: verifyMutationGate failing when score < break threshold
  let failedMsg = '';
  const mockContext = {
    repo: { owner: 'ericksonlopezf', repo: 'dotnet-value-objects' },
    sha: 'fail1234567890'
  };

  const mockGithub = {
    rest: {
      repos: {
        getCombinedStatusForRef: async () => ({
          data: {
            statuses: [
              {
                context: 'mutation-testing/stryker',
                state: 'failure',
                description: 'Stryker: 92.5% (200/216 killed) - ❌ FAILED',
                updated_at: new Date().toISOString(),
                target_url: 'https://github.com/ericksonlopezf/dotnet-value-objects/actions/runs/12346'
              }
            ]
          }
        })
      }
    }
  };

  const mockCore = {
    setFailed: (msg) => { failedMsg = msg; }
  };

  try {
    await verifyMutationGate({ github: mockGithub, context: mockContext, core: mockCore });
    assert.fail('Should have thrown an error for failing score');
  } catch (err) {
    assert.ok(err.message.includes('STRYKER GATE FAILED'), 'Expected failure error message');
    assert.ok(failedMsg.includes('STRYKER GATE FAILED'), 'Expected core.setFailed to be called');
  }
  console.log('✅ Test 5 Passed: verifyMutationGate correctly blocks release on score < 95%');
})
.then(async () => {
  // Test 6: verifyMutationGate fallback to main commit history when releasing on tag
  const mockContext = {
    repo: { owner: 'ericksonlopezf', repo: 'dotnet-value-objects' },
    sha: 'tagsha999999'
  };

  const freshDate = new Date().toISOString();

  const mockGithub = {
    rest: {
      repos: {
        getCombinedStatusForRef: async ({ ref }) => {
          if (ref === 'mainsha888888') {
            return {
              data: {
                statuses: [
                  {
                    context: 'mutation-testing/stryker',
                    state: 'success',
                    description: 'Stryker: 98.5% (200/203 killed) - 🟡 LOW',
                    updated_at: freshDate,
                    target_url: 'https://github.com/ericksonlopezf/dotnet-value-objects/actions/runs/12347'
                  }
                ]
              }
            };
          }
          return { data: { statuses: [] } };
        },
        listCommits: async () => ({
          data: [
            { sha: 'mainsha888888', commit: { committer: { date: freshDate } } }
          ]
        }),
        compareCommits: async () => ({
          data: {
            files: [
              { filename: 'CHANGELOG.md' },
              { filename: 'package.json' }
            ]
          }
        })
      }
    }
  };

  let failed = false;
  const mockCore = {
    setFailed: () => { failed = true; }
  };

  await verifyMutationGate({ github: mockGithub, context: mockContext, core: mockCore });
  assert.strictEqual(failed, false, 'Should pass by locating latest main commit status without src changes');
  console.log('✅ Test 6 Passed: verifyMutationGate correctly searches recent main commits for tag releases');
})
.then(async () => {
  // Test 7: verifyMutationGate rejects expired reports (> 7 days)
  const oldDate = new Date(Date.now() - 8 * 24 * 60 * 60 * 1000).toISOString(); // 8 days ago
  const mockContext = {
    repo: { owner: 'ericksonlopezf', repo: 'dotnet-value-objects' },
    sha: 'expired12345'
  };

  const mockGithub = {
    rest: {
      repos: {
        getCombinedStatusForRef: async () => ({
          data: {
            statuses: [
              {
                context: 'mutation-testing/stryker',
                state: 'success',
                description: 'Stryker: 100% - ✅ HIGH',
                updated_at: oldDate,
                target_url: 'https://github.com/ericksonlopezf/dotnet-value-objects/actions/runs/old'
              }
            ]
          }
        })
      }
    }
  };

  let failedMsg = '';
  const mockCore = {
    setFailed: (msg) => { failedMsg = msg; }
  };

  try {
    await verifyMutationGate({ github: mockGithub, context: mockContext, core: mockCore });
    assert.fail('Should have failed for expired report');
  } catch (err) {
    assert.ok(err.message.includes('expired'), 'Expected expired message');
    assert.ok(failedMsg.includes('expired'), 'Expected core.setFailed to receive expired message');
  }
  console.log('✅ Test 7 Passed: verifyMutationGate correctly rejects expired reports older than 7 days');
})
.then(async () => {
  // Test 8: verifyMutationGate rejects when code drift in src/ is detected
  const mockContext = {
    repo: { owner: 'ericksonlopezf', repo: 'dotnet-value-objects' },
    sha: 'newcode9999'
  };

  const freshDate = new Date().toISOString();

  const mockGithub = {
    rest: {
      repos: {
        getCombinedStatusForRef: async ({ ref }) => {
          if (ref === 'oldcommit1111') {
            return {
              data: {
                statuses: [
                  {
                    context: 'mutation-testing/stryker',
                    state: 'success',
                    description: 'Stryker: 100% - ✅ HIGH',
                    updated_at: freshDate,
                    target_url: 'https://github.com/ericksonlopezf/dotnet-value-objects/actions/runs/drift'
                  }
                ]
              }
            };
          }
          return { data: { statuses: [] } };
        },
        listCommits: async () => ({
          data: [
            { sha: 'oldcommit1111', commit: { committer: { date: freshDate } } }
          ]
        }),
        compareCommits: async () => ({
          data: {
            files: [
              { filename: 'src/EricksonLopez.ValueObjects/Money.cs' },
              { filename: 'README.md' }
            ]
          }
        })
      }
    }
  };

  let failedMsg = '';
  const mockCore = {
    setFailed: (msg) => { failedMsg = msg; }
  };

  try {
    await verifyMutationGate({ github: mockGithub, context: mockContext, core: mockCore });
    assert.fail('Should have failed for src/ code drift');
  } catch (err) {
    assert.ok(err.message.includes("Production code in 'src/' has changed"), 'Expected src/ drift message');
    assert.ok(failedMsg.includes("Production code in 'src/' has changed"), 'Expected core.setFailed drift message');
  }
  console.log('✅ Test 8 Passed: verifyMutationGate correctly rejects release when src/ code was modified');
  console.log('\n🎉 ALL JAVASCRIPT GATE VERIFICATION TESTS PASSED!');
})
.catch(err => {
  console.error('❌ Test failed:', err);
  process.exit(1);
});
