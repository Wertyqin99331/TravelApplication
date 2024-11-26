from collections import deque

n, m, q = map(int, input().split())
grid = [list(input().strip()) for _ in range(n)]
shots = [tuple(map(int, input().split())) for _ in range(q)]

directions = [(-1, 0), (1, 0), (0, -1), (0, 1)]
remaining_parts = {}

def bfs(x, y, ship_id):
    queue = deque([(x, y)])
    parts = 0
    while queue:
        cx, cy = queue.popleft()
        if grid[cx][cy] != 'X':
            continue
        grid[cx][cy] = ship_id
        parts += 1
        for dx, dy in directions:
            nx, ny = cx + dx, cy + dy
            if 0 <= nx < n and 0 <= ny < m and grid[nx][ny] == 'X':
                queue.append((nx, ny))
    remaining_parts[ship_id] = parts

ship_id = 1
for i in range(n):
    for j in range(m):
        if grid[i][j] == 'X':
            bfs(i, j, ship_id)
            ship_id += 1

for i, j in shots:
    i, j = i - 1, j - 1
    if grid[i][j] == '.':
        print("MISS")
        continue
    if isinstance(grid[i][j], int):
        ship_id = grid[i][j]
        remaining_parts[ship_id] -= 1
        if remaining_parts[ship_id] == 0:
            print("DESTROY")
        else:
            print("HIT")
