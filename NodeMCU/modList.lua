local List, modName = {}, ...

function List.init(maxSize)
    package.loaded[modName] = nil
    
    List.first = 0
    List.last = -1
    List.maxSize = maxSize
end

function List.size()
    return List.last - List.first + 1
end

function List.push(value)
    local last = List.last + 1
    List.last = last
    List[last] = value

    if List.size() > List.maxSize then
        List[List.first] = nil
        List.first = List.first + 1
    end
end

-- Get from end of list, 1 means last item. 2 means second last.
function List.getFromEnd(number)
    if number < 1 or number > List.size() then
        return nil
    end

    local index = List.last - number + 1
    return List[index]
end

return List
